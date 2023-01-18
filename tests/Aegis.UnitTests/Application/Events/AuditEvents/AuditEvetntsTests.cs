
namespace Aegis.UnitTests.Application.Events.AuditEvents
{
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;

	using global::Aegis.Application.Contracts.Application.Events;
	using global::Aegis.Application.Events.Audit.DataProtection;
	using global::Aegis.Application.Events.AuditLog.Handlers;
	using global::Aegis.Enums.AuditEvents;
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.Application;

	using MediatR;

	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Primitives;

	using Shouldly;

	public class AuditEvetntsTests
	{
		private readonly List<AuditLog> _audotLogs = new List<AuditLog>();
		private readonly Mock<ILogger<AuditEventHandler<IAuditEvent>>> _logger = new Mock<ILogger<AuditEventHandler<IAuditEvent>>>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();
		private readonly Mock<HttpRequest> _hcr = new Mock<HttpRequest>();
		private readonly Mock<ClaimsPrincipal> _hccp = new Mock<ClaimsPrincipal>();
		private readonly Mock<ConnectionInfo> _hcci = new Mock<ConnectionInfo>();
		private readonly Mock<IHttpContextAccessor> _hca = new Mock<IHttpContextAccessor>();
		private readonly Mock<IRepository<AuditLog>> _alRepo = new Mock<IRepository<AuditLog>>();
		private readonly Mock<IAegisContext> _ac = new Mock<IAegisContext>();

		public AuditEvetntsTests()
		{
			_alRepo.Setup(x => x.Create(It.IsAny<AuditLog>()))
				.Callback((AuditLog entity) => _audotLogs.Add(entity));
			_ac.Setup(x => x.AuditLogs)
				.Returns(_alRepo.Object);
			_hcr.Setup(x => x.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "User-Agent", new StringValues("Test") } }));
			_hcci.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));

			_hc.Setup(x => x.Request).Returns(_hcr.Object);
			_hc.Setup(x => x.User).Returns(_hccp.Object);
			_hc.Setup(x => x.Connection).Returns(_hcci.Object);
			_hca.Setup(x => x.HttpContext).Returns(_hc.Object);
		}

		[Fact]
		public void Handle_ShouldWork_ServiceInitiated()
		{
			// Arrange
			_audotLogs.Clear();
			Guid key = Guid.NewGuid();
			CreateLookupProtectionKeySucceededAuditEvent @event = new CreateLookupProtectionKeySucceededAuditEvent(key, "Test", true);
			AuditEventHandler<IAuditEvent> handler = new AuditEventHandler<IAuditEvent>(_logger.Object, _hca.Object, _ac.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(@event, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldBeNull();
			_audotLogs.Count.ShouldBe(1);
			AuditLog log = _audotLogs.First();
			log.ShouldNotBeNull();
			log.Module.ShouldBe((int)AuditModule.Application);
			log.Action.ShouldBe((int)AuditAction.Create);
			log.Subject.ShouldBe((int)AuditSubject.ProtectionKey);
			log.SubjectId.ShouldBe(key);
			log.Summary.ShouldBe("Test");
			log.EventName.ShouldBe(nameof(CreateLookupProtectionKeySucceededAuditEvent).Replace("AuditEvent", ""));
		}

		[Fact]
		public void Handle_ShouldWork_UserInitiated()
		{
			// Arrange
			_audotLogs.Clear();
			Guid key = Guid.NewGuid();
			Guid subId = Guid.NewGuid();
			_hccp.Setup(x => x.Claims).Returns(new List<Claim> { new Claim("sub", subId.ToString()), new Claim(type: "name", "Test") });
			CreateLookupProtectionKeyFailedAuditEvent @event = new CreateLookupProtectionKeyFailedAuditEvent(key, "Test");
			AuditEventHandler<IAuditEvent> handler = new AuditEventHandler<IAuditEvent>(_logger.Object, _hca.Object, _ac.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(@event, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldBeNull();
			_audotLogs.Count.ShouldBe(1);
			AuditLog log = _audotLogs.First();
			log.ShouldNotBeNull();
			log.Module.ShouldBe((int)AuditModule.Application);
			log.Action.ShouldBe((int)AuditAction.Create);
			log.Subject.ShouldBe((int)AuditSubject.ProtectionKey);
			log.SubjectId.ShouldBe(key);
			log.Summary.ShouldBe("Test");
			log.EventName.ShouldBe(nameof(CreateLookupProtectionKeyFailedAuditEvent).Replace("AuditEvent", ""));
			log.UserId.ShouldBe(subId);
			log.UserName.ShouldBe("Test");
			log.UserIp.ShouldBe("127.0.0.1");
			log.UserAgent.ShouldBe("Test");
		}

		[Fact]
		public void Handle_ShouldNotStop_OnException()
		{
			// Arrange
			Guid key = Guid.NewGuid();
			CreateLookupProtectionKeyFailedAuditEvent @event = new CreateLookupProtectionKeyFailedAuditEvent(key, "Test");
			AuditEventHandler<IAuditEvent> handler = new AuditEventHandler<IAuditEvent>(_logger.Object, _hca.Object, _ac.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(@event, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldBeNull();
			_audotLogs.Count.ShouldBe(0);
		}
	}
}
