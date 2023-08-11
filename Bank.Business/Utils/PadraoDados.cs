
using Bank.Business.Exception;
using Bank.Model;
using System.Text.RegularExpressions;

namespace Bank.Business.Utils
{
    public class PadraoDados
    {
        private static readonly PadraoDados padraoDados = new PadraoDados();

        public static PadraoDados InstanciaPadraoDados => padraoDados;

        public bool ValidaTresDigitos(string valor) => Regex.IsMatch(valor, @"^\d{3}$");

        public bool ValidaCincoDigitos(string valor) => Regex.IsMatch(valor, @"^\d{5}$");

        public bool ValidaNoveDigitos(string valor) => Regex.IsMatch(valor, @"^\d{9}$");

        public bool ValidaOnzeDigitos(string valor) => Regex.IsMatch(valor, @"^\d{11}$");

        public bool ValidaQuatorzeDigitos(string valor) => Regex.IsMatch(valor, @"^\d{14}$");

    }
}

