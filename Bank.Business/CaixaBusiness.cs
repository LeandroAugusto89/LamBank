
using Bank.Business.Utils;
using Bank.Model;

namespace Bank.Business
{
    public class CaixaBusiness
    {
        /// <summary>
        /// Valida se no caixa possui as notas requisitadas no saque
        /// </summary>
        /// <param name="cedulas">Código das cédulas</param>
        /// <param name=")">Valor do saque</param>

        private List<CedulasUtilitario> ValidaCedulas(List<CedulasUtilitario> cedulas, double saque)
        {

            //var cedulasNoCaixa = new List<Cedulas>()
            //{
            //    new Cedulas { Id = 1, Nome = "200 reais", Valor = 200.00, Quantidade = 0 },
            //    new Cedulas { Id = 2, Nome = "100 reais", Valor = 100.00, Quantidade = 10 },
            //    new Cedulas { Id = 3, Nome = "50 reais", Valor = 50.00, Quantidade = 10 },
            //    new Cedulas { Id = 4, Nome = "20 reais", Valor = 20.00, Quantidade = 10 },
            //    new Cedulas { Id = 5, Nome = "10 reais", Valor = 10.00, Quantidade = 10 },
            //    new Cedulas { Id = 6, Nome = "5 reais", Valor = 5.00, Quantidade = 10 },
            //    new Cedulas { Id = 7, Nome = "2 reais", Valor = 2.00, Quantidade = 10 }
            //};

            int saqueInt = Convert.ToInt32(saque);
            int resto = 0;
            int nota = 0;
            int qntEntregar = 0;
            int saqueMaximo = 0;
            int saqueAgora = 0;
            var listaCedulas = new List<CedulasUtilitario>();

            foreach (var cedula in cedulas)
            {
                if (cedula.Quantidade > 0)
                {
                    nota = Convert.ToInt32(cedula.Valor);
                    saqueMaximo = cedula.Quantidade * nota;
                    resto = saqueInt % nota;
                    if (nota == 5 && resto % 2 == 1)
                        continue;
                    saqueAgora = saqueMaximo > saqueInt ? saqueInt : saqueMaximo;
                    qntEntregar = saqueAgora / nota;
                    saqueInt = resto + (saqueInt - saqueAgora);

                    listaCedulas.Add(new CedulasUtilitario { Id = cedula.Id, Nome = cedula.Nome, Valor = cedula.Valor, Quantidade = qntEntregar });
                }
            }

            return listaCedulas;
        }
    }
}
