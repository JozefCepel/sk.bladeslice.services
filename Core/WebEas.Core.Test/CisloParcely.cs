using System.Linq;

namespace WebEas.Core.Test
{

    /// <summary>
    /// Trieda predstavuje cislo parcely.
    /// Ak je Pku NULL ide o parcelu typu C
    /// Ak je Pku cislo (moze byt aj nula) ide o parcelu typu E
    /// </summary>
    public class CisloParcely
    {
        private readonly int kmen;
        private readonly short podiel;
        private readonly byte diel;
        private readonly byte? pku;

        public CisloParcely(int kmen, short podiel, byte diel, byte? pku)
        {
            this.kmen = kmen;
            this.podiel = podiel;
            this.diel = diel;
            this.pku = pku;
        }


        /// <summary>
        /// Parsuje cislo parcely zo stringu v tvare [pku-]kmen[/podiel[/diel]] 
        /// </summary>
        public static bool TryParse(string kodParcely, out CisloParcely cisloParcely)
        {
            int kmen;
            short podiel = 0;
            byte diel = 0;
            byte? pku = null;

            cisloParcely = null;

            if (string.IsNullOrWhiteSpace(kodParcely))
                return false;

            if (kodParcely.Contains("/"))
            {
                var castiKodu = kodParcely.Split('/');
                if (castiKodu.Count() > 3)
                    return false;

                if (castiKodu.Count() == 3 && !byte.TryParse(castiKodu[2].Trim(), out diel))
                    return false;

                if (castiKodu.Count() >= 2 && !short.TryParse(castiKodu[1].Trim(), out podiel))
                    return false;

                var prvaCastKodu = castiKodu[0];

                if (prvaCastKodu.Contains("-"))
                {
                    var zlozkyPrvejCastiKodu = prvaCastKodu.Split('-');
                    if (zlozkyPrvejCastiKodu.Count() != 2)
                        return false;

                    byte pkuOut;
                    if (!byte.TryParse(zlozkyPrvejCastiKodu[0].Trim(), out pkuOut))
                        return false;

                    pku = pkuOut;

                    if (!int.TryParse(zlozkyPrvejCastiKodu[1].Trim(), out kmen))
                        return false;
                }
                else
                {
                    if (!int.TryParse(castiKodu[0].Trim(), out kmen))
                        return false;
                }
            }
            else
            {
                if (!int.TryParse(kodParcely.Trim(), out kmen))
                    return false;
            }

            cisloParcely = new CisloParcely(kmen, podiel, diel, pku);
            return true;
        }

        public int Kmen { get { return kmen; } }
        public short Podiel { get { return podiel; } }
        public byte Diel { get { return diel; } }
        public byte? Pku { get { return pku; } }
    }
}