using System;
using System.Linq;

namespace WebEas.Context
{
    public enum EndpointType : byte
    {
        Public = 0x56,
        Private = 0x50,
        Formulare = 0x46,
        Office = 0x4f,
        Unknown = 0x00,
    }
}