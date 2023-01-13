namespace Aegis.Application.Services.DataProtection
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// Aegis Lookup Protector KeyRing
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.ILookupProtector" />
	public sealed class AegisInternalLookupProtectorKeyRing : ILookupProtectorKeyRing
	{
		/// <summary>
		/// The random
		/// </summary>
		private readonly Random _rnd = new Random();

		/// <summary>
		/// The key ring
		/// </summary>
		private readonly Dictionary<string, byte[]> _keyRing = new Dictionary<string, byte[]>();

		/// <summary>
		/// Get the current key id.
		/// </summary>
		public string CurrentKeyId => _keyRing.Keys.ElementAt(_rnd.Next(_keyRing.Count));

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisInternalLookupProtectorKeyRing"/> class.
		/// </summary>
		public AegisInternalLookupProtectorKeyRing()
			=> this.InitializeKeys();

		/// <summary>
		/// Gets the <see cref="string"/> with the specified key identifier.
		/// </summary>
		/// <value>
		/// The <see cref="string"/>.
		/// </value>
		/// <param name="keyId">The key identifier.</param>
		/// <returns></returns>
		public string this[string keyId]
			=> Convert.ToBase64String(_keyRing[keyId]);

		/// <summary>
		/// Return all of the key ids.
		/// </summary>
		/// <returns>
		/// All of the key ids.
		/// </returns>
		public IEnumerable<string> GetAllKeyIds()
			=> _keyRing.Select(x => x.Key);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
		/// <returns></returns>
		private void InitializeKeys()
		{
			byte[,] keyBytes = KeyBytes();
			IEnumerable<int[]> keyCombinations = KeyCombination();

			int keyCounter = 0;

			foreach (int[] combiantion in keyCombinations)
			{
				keyCounter++;

				byte[] key = new byte[32];

				for (int b = 0; b < 8; b++)
				{
					int kIndex = b;

					if (kIndex != 0)
					{
						kIndex *= 4;
					}

					key[kIndex] = keyBytes[combiantion[0], b];
					key[kIndex + 1] = keyBytes[combiantion[1], b];
					key[kIndex + 2] = keyBytes[combiantion[2], b];
					key[kIndex + 3] = keyBytes[combiantion[3], b];
				}

				string keyHash = GetHash(key);
				_keyRing.Add(keyHash, key);
			}
		}

		/// <summary>
		/// Gets the hash.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		private static string GetHash(byte[] key)
		{
			StringBuilder builder = new StringBuilder();
			byte[] hashBytes = SHA256.HashData(key);

			for (int h = 0; h < hashBytes.Length; h++)
			{
				builder.Append(hashBytes[h].ToString("x2"));
			}

			return builder.ToString();
		}

		/// <summary>
		/// Keys the combination.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<int[]> KeyCombination()
		{
			HashSet<int> visited = new HashSet<int>();
			int[] nums = new int[] { 0, 1, 2, 3 };
			List<int[]> result = new List<int[]>();

			for (int k = 0; k < nums.Length; k++)
			{
				for (int j = 0; j < nums.Length; j++)
				{
					int indexCombination = k + j;

					if (visited.Contains(indexCombination) || k == j)
					{
						continue;
					}

					visited.Add(indexCombination);

					int[] perm = new int[nums.Length];
					Array.Copy(nums, perm, nums.Length);

					perm[k] = nums[j];
					perm[j] = nums[k];

					result.Add(perm);
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the key bytes.
		/// </summary>
		/// <returns></returns>
		private static byte[,] KeyBytes() => new byte[,]
			{
				 { 84, 211, 252, 195, 159, 41, 9, 93 },
				 { 219, 249, 88, 24, 207, 85, 58, 31 },
				 { 225, 136, 194, 170, 187, 13, 167, 176 },
				 { 251, 61, 84, 43, 145, 65, 93, 70 }
			};
	}
}
