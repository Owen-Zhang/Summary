js 加密
import CryptoJS from 'crypto-js'

const key = CryptoJS.enc.Utf8.parse("1234567891234567");  //十六位十六进制数作为密钥
const iv = CryptoJS.enc.Utf8.parse('1234567891234567');   //十六位十六进制数作为密钥偏移量

let srcs = CryptoJS.enc.Utf8.parse("123456789");
let encrypted = CryptoJS.AES.encrypt(srcs, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
var secret = encrypted.ciphertext.toString().toUpperCase();
console.log(secret);

let encryptedHexStr = CryptoJS.enc.Hex.parse(secret);
let srcs2 = CryptoJS.enc.Base64.stringify(encryptedHexStr);
let decrypt = CryptoJS.AES.decrypt(srcs2, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
let decryptedStr = decrypt.toString(CryptoJS.enc.Utf8);
console.log(decryptedStr.toString());


golang 解密

src2, _ := hex.DecodeString("7CD0DE993D05DC019C1100E838BD9220")
result, err := file.Decrypt(src2, []byte("1234567891234567"))
if err != nil {
	fmt.Println(err)
	return
}
fmt.Println(string(result))

--------------------
package file

import (
	"bytes"
	"crypto/aes"
	"crypto/cipher"
)

func Decrypt(ciphertext, key []byte) ([]byte, error) {
	pkey := PaddingLeft(key, '0', 16)
	block, err := aes.NewCipher(pkey)
	if err != nil {
		return nil, err
	}
	blockModel := cipher.NewCBCDecrypter(block, pkey)
	plantText := make([]byte, len(ciphertext))
	blockModel.CryptBlocks(plantText, []byte(ciphertext))
	plantText = PKCS7UnPadding(plantText, block.BlockSize())
	return plantText, nil
}

func PKCS7UnPadding(plantText []byte, blockSize int) []byte {
	length := len(plantText)
	unpadding := int(plantText[length-1])
	return plantText[:(length - unpadding)]
}

func PaddingLeft(ori []byte, pad byte, length int) []byte {
	if len(ori) >= length {
		return ori[:length]
	}
	pads := bytes.Repeat([]byte{pad}, length-len(ori))
	return append(pads, ori...)
}
