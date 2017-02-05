using System;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;
using PCLCrypto;

namespace MemberAdministration
{
	public class PasswordHashingService
	{
		char delimiter = ':';
		int saltByteSize = 32;
		int iterations = 10000;
		int keyLengthInBytes = 20; //https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2 => With PBKDF2-SHA1 this is 160 bits or 20 bytes
		string hashAlgorithmForSecureRandom = "SHA512";

		readonly ISettingsProxy _settingsProxy;

		public PasswordHashingService(ISettingsProxy settingsProxy)
		{
			_settingsProxy = settingsProxy;
		}

		public async Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash)
		{
			//string hashedClientInput = await HashPasswordAsync(plainTextPassword);

			byte[] storedSalt = GetSalt(storedPasswordHash);

			string enterdPasswordHash = await HashPasswordAsync(enteredPassword, storedSalt);

			return enterdPasswordHash == storedPasswordHash;
		}

		byte[] GetSalt(string encryptedPassword)
		{
			var passwordParameters = encryptedPassword.Split(delimiter);

			string storedSalt = passwordParameters[1];

			byte[] saltBytes = Convert.FromBase64String(storedSalt);

			return saltBytes;
		}

		public async Task<string> HashPasswordAsync(string password, byte[] saltBytes = null)
		{
			string pepper = App.AppId;
			hashAlgorithmForSecureRandom = (await _settingsProxy.GetSettingAsync("HashAlgorithm")).Value;

			if (saltBytes == null)
			{
				saltBytes = GenerateSecureRandomSalt();
			}

			//uses Pbkdf2
			byte[] deriveBytes = NetFxCrypto.DeriveBytes.GetBytes(password + pepper, saltBytes, iterations, keyLengthInBytes);

			string hash = Convert.ToBase64String(deriveBytes);
			string salt = Convert.ToBase64String(saltBytes);
			string stringToStore = $"{hash}{delimiter}{salt}";

			return stringToStore;
		}

		byte[] GenerateSecureRandomSalt()
		{
			byte[] saltBytes = new byte[saltByteSize];
			var secureRandom = SecureRandom.GetInstance($"{hashAlgorithmForSecureRandom}PRNG", true);
			secureRandom.NextBytes(saltBytes);
			return saltBytes;
		}
}
}
