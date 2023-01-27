
namespace Aegis.UnitTests.Application.Events.AuditEvents
{
	using global::Aegis.Application.Contracts.Application.Events;
	using global::Aegis.Application.Events.Audit.DataProtection;
	using global::Aegis.Application.Events.AuditLog.Handlers;
	using global::Aegis.Enums.AuditEvents;
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.Application;

	using Microsoft.Extensions.Primitives;

	public class AuditEvetntsTests
	{
		private static readonly Faker _faker = new Faker("en");

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
			_hcr.Setup(x => x.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "User-Agent", new StringValues(_faker.Internet.UserAgent()) } }));
			_hcci.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(_faker.Internet.Ip()));

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
			Guid key = _faker.Random.Guid();
			string summary = _faker.Random.String2(36);
			CreateLookupProtectionKeySucceededAuditEvent @event = new CreateLookupProtectionKeySucceededAuditEvent(key, summary, true);
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
			log.Summary.ShouldBe(summary);
			log.EventName.ShouldBe(nameof(CreateLookupProtectionKeySucceededAuditEvent).Replace("AuditEvent", ""));
		}

		[Fact]
		public void Handle_ShouldWork_UserInitiated()
		{
			// Arrange
			_audotLogs.Clear();
			Guid key = _faker.Random.Guid();
			Guid subId = _faker.Random.Guid();
			string userName = _faker.Internet.UserName();
			string summary = _faker.Random.String2(36);
			_hccp.Setup(x => x.Claims).Returns(new List<Claim> { new Claim("sub", key.ToString()), new Claim(type: "name", userName) });
			CreateLookupProtectionKeyFailedAuditEvent @event = new CreateLookupProtectionKeyFailedAuditEvent(subId, summary);
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
			log.SubjectId.ShouldBe(subId);
			log.Summary.ShouldBe(summary);
			log.EventName.ShouldBe(nameof(CreateLookupProtectionKeyFailedAuditEvent).Replace("AuditEvent", ""));
			log.UserId.ShouldBe(key);
			log.UserName.ShouldBe(userName);
			log.UserIp.ShouldNotBeNull();
			log.UserAgent.ShouldNotBeNull();
		}
		[Fact]
		public void Handle_ShouldWork_UserInitiated_NoRemoteIp()
		{
			// Arrange
			_audotLogs.Clear();
			Guid key = _faker.Random.Guid();
			Guid subId = _faker.Random.Guid();
			string userName = _faker.Internet.UserName();
			string summary = _faker.Random.String2(36);
			_hccp.Setup(x => x.Claims).Returns(new List<Claim> { new Claim("sub", subId.ToString()), new Claim(type: "name", userName) });
			_hcci.Setup(x => x.RemoteIpAddress).Returns((IPAddress?)null);
			CreateLookupProtectionKeyFailedAuditEvent @event = new CreateLookupProtectionKeyFailedAuditEvent(key, summary);
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
			log.Summary.ShouldBe(summary);
			log.EventName.ShouldBe(nameof(CreateLookupProtectionKeyFailedAuditEvent).Replace("AuditEvent", ""));
			log.UserId.ShouldBe(subId);
			log.UserName.ShouldBe(userName);
			log.UserIp.ShouldBeNull();
			log.UserAgent.ShouldNotBeNull();
		}
		[Fact]
		public void Handle_ShouldNotStop_OnException()
		{
			// Arrange
			_audotLogs.Clear();
			Guid key = _faker.Random.Guid();
			string summary = _faker.Random.String2(36);
			CreateLookupProtectionKeyFailedAuditEvent @event = new CreateLookupProtectionKeyFailedAuditEvent(key, summary);
			AuditEventHandler<IAuditEvent> handler = new AuditEventHandler<IAuditEvent>(_logger.Object, _hca.Object, _ac.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(@event, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldBeNull();
			_audotLogs.Count.ShouldBe(0);
		}
	}
}
