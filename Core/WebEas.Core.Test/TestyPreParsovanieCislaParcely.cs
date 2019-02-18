using NUnit.Framework;

namespace WebEas.Core.Test
{
    [TestFixture]
    public class TestyPreParsovanieCislaParcely
    {
        [TestCase("1")]
        [TestCase("  1")]
        [TestCase("1 ")]
        [TestCase(" 1  ")]
        public void ZadaneJednoCislo_VyparsujeKmen_DielPodielJeNula_PkuJeNull(string kodParcely)
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(kodParcely, out cisloParcely);

            Assert.That(response, Is.True);
            Assert.That(cisloParcely.Kmen, Is.EqualTo(1));
            Assert.That(cisloParcely.Podiel, Is.EqualTo(0));
            Assert.That(cisloParcely.Diel, Is.EqualTo(0));
            Assert.That(cisloParcely.Pku, Is.Null);
        }

        [TestCase("11/11")]
        [TestCase("  11/11")]
        [TestCase("11 /11")]
        [TestCase(" 11  /11")]
        [TestCase("11/ 11")]
        [TestCase("11/11 ")]
        [TestCase("11/ 11 ")]
        public void ZadaneDveCiselneCasti_VyparsujeKmenPodiel_DielJeNula_PkuJeNull(string kodParcely)
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(kodParcely, out cisloParcely);

            Assert.That(response, Is.True);
            Assert.That(cisloParcely.Kmen, Is.EqualTo(11));
            Assert.That(cisloParcely.Podiel, Is.EqualTo(11));
            Assert.That(cisloParcely.Diel, Is.EqualTo(0));
            Assert.That(cisloParcely.Pku, Is.Null);            
        }

        [TestCase("111/111/111")]
        [TestCase(" 111/111/111")]
        [TestCase("111  /111/111")]
        [TestCase(" 111  /111/111")]
        [TestCase("111/ 111/111")]
        [TestCase("111/111  /111")]
        [TestCase("111/  111 /111")]
        [TestCase("111/111/ 111")]
        [TestCase("111/111/111  ")]
        [TestCase("111/111/  111 ")]
        public void ZadaneTriCiselneCasti_VyparsujeKmenPodielDiel_PkuJeNull(string kodParcely)
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(kodParcely, out cisloParcely);

            Assert.That(response, Is.True);
            Assert.That(cisloParcely.Kmen, Is.EqualTo(111));
            Assert.That(cisloParcely.Podiel, Is.EqualTo(111));
            Assert.That(cisloParcely.Diel, Is.EqualTo(111));
            Assert.That(cisloParcely.Pku, Is.Null);            
        }

        [TestCase("1-1/1/1")]
        [TestCase("1 -1/1/1")]
        [TestCase(" 1-1/1/1")]
        [TestCase("  1-1 /1/1")]
        [TestCase(" 1-1 /1/1")]
        [TestCase("1 - 1/1/1")]
        [TestCase(" 1 - 1 /1/1")]
        public void ZadanyCelyKod_VyparsujeKmenPodielDielPku(string kodParcely)
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(kodParcely, out cisloParcely);

            Assert.That(response, Is.True);
            Assert.That(cisloParcely.Kmen, Is.EqualTo(1));
            Assert.That(cisloParcely.Podiel, Is.EqualTo(1));
            Assert.That(cisloParcely.Diel, Is.EqualTo(1));
            Assert.That(cisloParcely.Pku, Is.EqualTo(1));            
        }

        [Test]
        public void VstupJeEmptyString_VratiFalseZiadnaException()
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse("", out cisloParcely);

            Assert.That(response, Is.False);
            Assert.That(cisloParcely, Is.Null);            
        }

        [Test]
        public void VstupJeEmptyNull_VratiFalseZiadnaException()
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(null, out cisloParcely);

            Assert.That(response, Is.False);
            Assert.That(cisloParcely, Is.Null);            
        }

        [Test]
        public void VstupObsahujePrilisVelaCasti_VratiFalseZiadnaException()
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse("1-1/1/1/1", out cisloParcely);

            Assert.That(response, Is.False);
            Assert.That(cisloParcely, Is.Null);
        }

        [TestCase("a")]
        [TestCase("1/a")]
        [TestCase("1/1/a")]
        [TestCase("1/a/1")]
        [TestCase("a/1/1")]
        [TestCase("1-a/1/1")]
        [TestCase("1-1/a/1")]
        [TestCase("1-1/1/a")]
        [TestCase("a-1/1/1")]
        public void NiektoraZoZloziekNiejeParsovatelnaNaCislo_VratiFalseZiadnaException(string kodParcely)
        {
            CisloParcely cisloParcely;
            var response = CisloParcely.TryParse(kodParcely, out cisloParcely);

            Assert.That(response, Is.False);
            Assert.That(cisloParcely, Is.Null);
        }
    }
}
