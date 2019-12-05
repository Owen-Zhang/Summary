package encypt

import (
	"crypto/rand"
	"crypto/rsa"
	"crypto/x509"
	"encoding/base64"
	"encoding/pem"
	"fmt"
	"os"
)

func GenerateKeys() {
	//得到私钥
	privateKey, _ := rsa.GenerateKey(rand.Reader, 2048)
	//通过x509标准将得到的ras私钥序列化为ASN.1 的 DER编码字符串
	x509_Privatekey := x509.MarshalPKCS1PrivateKey(privateKey)
	//创建一个用来保存私钥的以.pem结尾的文件
	fp, _ := os.Create("test_private.pem")
	defer fp.Close()
	//将私钥字符串设置到pem格式块中
	pem_block := pem.Block{
		Type:  "test_privateKey",
		Bytes: x509_Privatekey,
	}
	//转码为pem并输出到文件中
	pem.Encode(fp, &pem_block)

	//处理公钥,公钥包含在私钥中
	publickKey := privateKey.PublicKey
	//接下来的处理方法同私钥
	//通过x509标准将得到的ras私钥序列化为ASN.1 的 DER编码字符串
	x509_PublicKey, _ := x509.MarshalPKIXPublicKey(&publickKey)
	pem_PublickKey := pem.Block{
		Type:  "test_PublicKey",
		Bytes: x509_PublicKey,
	}
	file, _ := os.Create("test_PublicKey.pem")
	defer file.Close()
	//转码为pem并输出到文件中
	pem.Encode(file, &pem_PublickKey)
}

func RsaEncrypter(path string, msg []byte) []byte {
	//首先从文件中提取公钥
	fp, _ := os.Open(path)
	defer fp.Close()
	//测量文件长度以便于保存
	fileinfo, _ := fp.Stat()
	buf := make([]byte, fileinfo.Size())
	fp.Read(buf)
	//下面的操作是与创建秘钥保存时相反的
	//pem解码
	block, _ := pem.Decode(buf)
	//x509解码,得到一个interface类型的pub
	pub, _ := x509.ParsePKIXPublicKey(block.Bytes)
	//加密操作,需要将接口类型的pub进行类型断言得到公钥类型
	cipherText, _ := rsa.EncryptPKCS1v15(rand.Reader, pub.(*rsa.PublicKey), msg)
	return cipherText
}

//RsaDecrypter 使用私钥进行解密
func RsaDecrypter(path string, cipherText []byte) []byte {
	//同加密时，先将私钥从文件中取出，进行二次解码
	fp, _ := os.Open(path)
	defer fp.Close()
	fileinfo, _ := fp.Stat()
	buf := make([]byte, fileinfo.Size())
	fp.Read(buf)
	block, _ := pem.Decode(buf)
	PrivateKey, _ := x509.ParsePKCS1PrivateKey(block.Bytes)
	//二次解码完毕，调用解密函数
	afterDecrypter, _ := rsa.DecryptPKCS1v15(rand.Reader, PrivateKey, cipherText)
	return afterDecrypter
}

func TestRsa() {
	// msg := "测试rsa加密"
	// data := RsaEncrypter("test_PublicKey.pem", []byte(msg))
	// encyptContent := hex.EncodeToString(data)
	// fmt.Println(encyptContent)

	// decodeContent, err := hex.DecodeString(encyptContent)
	// if err != nil {
	// 	fmt.Println(err)
	// 	return
	// }
	
	//前端传回的根据公钥加密的字符串信息
	decodeContent, err := base64.StdEncoding.DecodeString("l61HgZj8mVl89k/qu4c+o3h0SCcusLann4MRgIb0vdrrJdEKejuajVjUhAUuN6Rm4bwH4i6yy9lXXDlMvhH5sKsCiTk0h5zZmadpH0/wgPFLkw7H1UL8yxsBQmXIi2MA1yDy9GU8FLOnhpE9++9Em1YvVMb+ukq21/8TKmDuBPd2Zg5j2lLbAyoZEbxEgEMrC0NcopU3FLzZUTgCb4JzjEQ6J7tzK9tewbqFAWvFhy2DcJnqw8GOEdVfyOFC1mNuF+etjz3RWxpF77ip9T9dCEg0OQfC51CV0045q60mv0UtjXxZBiQShP6sOGYEtozFp0XYgn6kJeqawc9teo76iw==")
	if err != nil {
		fmt.Sprintln(err)
		return
	}
	result := RsaDecrypter("test_private.pem", decodeContent)
	fmt.Println(string(result))
}

//私钥信息如下:
-----BEGIN test_privateKey-----
MIIEogIBAAKCAQEArh1zeT7t+x5xGuTD7SU92eLF9VPjfLZQcyifkiyQ21p1fpDt
Vdq0PZ75RkVGpbemhg4UruFFtnlpviSqY8cE+xdf8fYdb69fprEA/nSbymt8mjVq
pFvEK3an/Jn1N1/N2am0kR3f8eXURDOmDhTzJVIamkMAHszatpwusnoNpgXKIZ2t
3Ler3fqBnx9pjMV0tUlZfutb80dIwPODNkgk9DVF5sm1IHLDRFTGDfVyFrA/YP9l
F3yTJGvk/SuTMrCDvuW1jDdP9dpkgyt+ZpvzIuKHEn7p7C2l1pc68i7lAB39wVhu
rUzpYhGKCrHvqvncXcBvNcR7Bh73WKQJpqLgWQIDAQABAoIBAC0sMOhU/lM3e6q5
jWd6UQi1gBx9Djkt58No17WJ4G84keErYpfyrO6Lzyp/EKwv8IE5J/6TLY4BBAEz
7A0E9nLahqb07oAqZMuPgq2BvWWP63zqvOc1c7i/CShNHUSnk3WUROedc5uDwEWN
jNh8cPTOz14UYfSbu2bDMI5XgjD/Q7JieTDPGRbj9NHH2s+dfPf5de635cJ1y7Vv
DJUdr5Oj57gpl0IjywbIxwhZvFbrRmKRt0PRDH7AkSHJfr3kRbtpJMmQP3JJ/FoV
3eaS6Ip8RQagWJsJWk6wiXXvDR91LXLXAsDoYv1k9iUuFXQpbvlxcZKu/GcAft0Y
3X4PurECgYEAxF8UzYwRTsvRSDlSebYZcvwZqe/glNlegEKWYY0P3RCCxpvhuVer
eHk4spLPd8Td21jz+mTLYfwUEMShEcT8hx6teMzxXg6RrtJ54qf78r8ICb317C7g
gS9Ff/QqxsGac/2FcQyMWqfdI8gNPcUzcEXvdP77fIp0YEkpk8eufHUCgYEA4vxF
wyHGkfp99vVw9wNmNm2VMiNgxXhCCJlsYhV4Op67lfN8Q+//wBJ+cNeoLFOLRqEL
OTsBhuumluwASDG4FneOChZq8+2OUd3jnDCO0hn5yJzUSJfMC+Kz//FBfHkbnzRk
aMit1EZsalLHtL1OSXsTSNXcNFbTUaw3lk2Rp9UCgYBqmE5bkXfntP3C3dLmXLIN
18k6lQrs4d3Jc9vb0k3VK1xB4WYzTOK4f90GGmliU0w3AF9YAZTheIuP2pywX6TQ
2BdEZsNy0ifEpV4iaht61rXPS/2Nmpilp3prjagWwgtMgUcJac1afJqvDK4bZMua
W2wzryHmpeWsSqq2HIOb7QKBgGMtLOIcsOF5nG46Qbh8EL35VXYJxS+i4t9VAek9
TkH2tynGsGYSyCFJM1vkroNnoXQjy36fDITCFBDfXyQsLS4L4NEBiIu6ITQeCmRl
RxH/7Ya9F3f8c1hCHrnW1PEpDWubfb/W5zTX7GjscHvFNx6eGwf5AXUQ9tY5tDQF
An5dAoGAQ2kxxRGNFzp+BcaFTtvfbuhMg0a87RJ7N6Z9+syTiro0fhVI1UjIv552
ie5aXqz+rtbB426dRxcqCxWaLUnPuF+HbOjXWnv1t0Z5vuzK+BDRIyhaHh32rcZh
2LIlj4igjgEXDNuz2Kc15Mtrb0bTz14kk7y4zeRDrKMBfm53IUQ=
-----END test_privateKey-----
