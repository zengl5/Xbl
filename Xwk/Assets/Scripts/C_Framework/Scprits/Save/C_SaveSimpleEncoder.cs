using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.C_Framework
{
    public class C_SaveSimpleEncoder : C_ISaveEncoder
    {
        public byte[] Encode(byte[] input, string password)
        {
            return C_XXTea.Encrypt(input, System.Text.Encoding.UTF8.GetBytes(password));
        }

        public byte[] Decode(byte[] input, string password)
        {
            return C_XXTea.Decrypt(input, System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}