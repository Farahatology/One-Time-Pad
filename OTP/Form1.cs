using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private byte[] pad;
        private void button1_Click(object sender, EventArgs e)
        {
            // Converting text to bytes, assuming unicode.
            byte[] originalBytes = Encoding.Unicode.GetBytes(textBoxOriginal.Text);

            // generate a pad in memory.
            pad = GeneratePad(size: originalBytes.Length, seed: 1);

            // I'm going to display these bytes in Base64, but one would
            // probably save them to a file; this is the Pad (or "key").
            textBoxPad.Text = Convert.ToBase64String(inArray: pad);

            // We encrypt the bytes by adding our noise.
            byte[] encrypted = Encrypt(originalBytes, pad);
            // again, displaying in base64, but you would typically save
            // these to a file too; this is your encrypted "file" or message.
            textBoxEncrypted.Text = Convert.ToBase64String(inArray: encrypted);
        }




        public static byte[] GeneratePad(int size, int seed)
        {
            var random = new Random(Seed: seed);
            var bytesBuffel = new byte[size];

            random.NextBytes(bytesBuffel);

            return bytesBuffel;
        }
        public static byte[] Encrypt(byte[] data, byte[] pad)
        {
            var result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var sum = (int)data[i] + (int)pad[i];
                if (sum > 255)
                    sum -= 255;
                result[i] = (byte)sum;
            }
            return result;
        }
        public static byte[] Decrypt(byte[] encrypted, byte[] pad)
        {
            var result = new byte[encrypted.Length];
            for (int i = 0; i < encrypted.Length; i++)
            {
                var dif = (int)encrypted[i] - (int)pad[i];
                if (dif < 0)
                    dif += 255;
                result[i] = (byte)dif;
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            byte[] encryptedFromBase64 = Convert.FromBase64String(textBoxEncrypted.Text);

            // decrypting the encoded message using the key made up of noise.
            byte[] decrypted = Decrypt(encryptedFromBase64, pad);

            // displaying the original unencrypted message.
            textBoxDecrypted.Text = Encoding.Unicode.GetString(decrypted);
        }
    }
}
