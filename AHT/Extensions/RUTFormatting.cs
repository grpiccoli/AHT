﻿using System;
using System.Globalization;

namespace ConsultaMD.Extensions
{
    public static class RUT
    {
        public static string DV(int rut)
        {
            int Id = rut;
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            string RutDigito;

            Contador = 2;
            Acumulador = 0;

            while (Id != 0)
            {
                Multiplo = (Id % 10) * Contador;
                Acumulador += Multiplo;
                Id /= 10;
                Contador += 1;
                if (Contador == 8)
                {
                    Contador = 2;
                }
            }
            Digito = 11 - (Acumulador % 11);
            RutDigito = Digito.ToString(CultureInfo.InvariantCulture).Trim();
            if (Digito == 10)
            {
                RutDigito = "K";
            }
            if (Digito == 11)
            {
                RutDigito = "0";
            }
            return (RutDigito);
        }
        public static string Format(string rut, bool thousandSep = true, bool dash = true)
        {
            var ruT = Unformat(rut);
            return Format(ruT.Value.rut, thousandSep, dash);
        }
        public static string Format(int rut, bool thousandSep = true, bool dash = true)
        {
            return $@"{(thousandSep ? 
                rut.ToString("N0", new CultureInfo("es-CL")) : 
                rut.ToString(CultureInfo.InvariantCulture) )}{(dash ? "-" : "")}{DV(rut)}";
        }
        public static string Fonasa(int rut)
        {
            return $"{rut.ToString("D10", CultureInfo.InvariantCulture)}-{DV(rut)}";
        }
        public static string Fonasa(string rut)
        {
            var unformated = Unformat(rut);
            return Fonasa(unformated.Value.rut);
        }
        public static bool IsValid(int rut, string dv) {
            var Dv = DV(rut);
            return Dv == dv;
        }
        public static bool IsValid(string rut)
        {
            return Unformat(rut) != null;
        }
        public static (int rut, string dv)? Unformat(this string formatted)
        {
            var array = formatted?.Replace(".", "", StringComparison.InvariantCulture).Split("-");
            var parsed = int.TryParse(array[0], out int rut);
            if (parsed)
            {
                if(DV(rut) == array[1].ToUpper(new CultureInfo("es-CL"))) return (rut, dv: array[1]);
            }
            return null;
        }
        public static int RutToInt(this string formatted) => formatted.Unformat().Value.rut;
    }
}
