using System.Security.Cryptography;
using System.Text;

namespace Diethynylbenzene.Source.Utils
{
	public class Cryptography
	{
		private static readonly string IV = new(new Char[]{(char)((0x1CC0^0x5)+(1<<2)-(3*2)+16),(char)((0x2D00|0x27)+((3*7)%5)+(4<<1)),(char)(((0x1800+0x44)^0x2)+(10/2)),(char)((90-(2*12)+(3<<1)-(7%4))),(char)((0xA470+0xD)^(4|8)+(9*3/2)),(char)(((0x2E00+0x44)^(1<<3))-(5/5))});
		private static readonly string KEY = new(new Char[]{(char)(((((0x2070*2)/4)^0x7)+0x5)+((1<<2)&0x3)),(char)((((0x750+0x23F)^0x10)-(5/2))^(1<<4)),(char)((((0x2580*2)/4)+0x8)^(1<<3)),(char)((0x16C0^0x33)+(7<<2)-(5+0x3)),(char)(((((0x0C80+2*3)^0x5)*2)/5)+0x1),(char)((((0x28A0|0x1F)-(8/4))^0x3)+((1<<3)&0xF)),(char)((0x1970^0xF)+((2<<2)&0x3)-(3/1)),(char)((((70+(3<<1))*3)/2)-(1<<2)),(char)((0x13C0^((0xA<<1)|0x4))+((5/5)<<1)),(char)((0x2D00^0x33)+((5<<2)-3)+(1<<1)),(char)((((0x30+3)*2)/3)^(2<<1)),(char)((((0x1F30+7)^(2<<3))*2)/3),(char)(((((0x32FE-0x10)^0x7)*2)/4)+(7%3)),(char)((0x3A+(5>>1))^((3<<3)/2)),(char)(((((0x1F600^0x3D)+(10<<2))*2)/5)),(char)((0x0D38+2*3)^(5/5)),(char)((((0x12400+0x91)^(0x3<<2))*3)/5)});

		public string Decrypt(string data)
		{
			using var aes = Aes.Create();
			aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(KEY));
			aes.IV = Encoding.UTF8.GetBytes(IV);

			byte[] bytes = Convert.FromBase64String(data);

			using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
			using var memoryStream = new MemoryStream(bytes);
			using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			using var streamReader = new StreamReader(cryptoStream);

			return streamReader.ReadToEnd();
		}
	}
}