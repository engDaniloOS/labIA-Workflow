using System.ComponentModel;

namespace labIA_Workflow.Tools.Mock
{
    public class EstoqueTool
    {
        [Description("Retorna caracteristicas do estoque de um produto.")]
        public string ConsultarEstoque([Description("Nome do produto, ex: Mouse, Teclado, Monitor")] string produto)
        {
            var estoque = new Dictionary<string, int>
            {
                { "Mouse", 10 },
                { "Teclado", 5 },
                { "Monitor", 0 }
            };

            if (estoque.TryGetValue(produto, out int quantidade))
                return $"O estoque do {produto} é: {quantidade}";

            return $"Produto {produto} não encontrado no estoque.";
        }

        [Description("Retorna o preço de um produto.")]
        public string ConsultarPreco(
            [Description("Nome do produto, ex: Mouse, Teclado, Monitor")] string produto)
        {
            var precos = new Dictionary<string, double>
            {
                { "Mouse", 19.99d },
                { "Teclado", 29.99d },
                { "Monitor", 1139.99d }
            };

            if (precos.TryGetValue(produto, out double preco))
                return $"O preço do {produto} é: R${preco}";

            return $"Produto {produto} não encontrado na lista de preços.";
        }
    }
}
