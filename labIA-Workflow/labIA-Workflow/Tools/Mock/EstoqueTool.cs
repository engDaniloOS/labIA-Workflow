using System.ComponentModel;

namespace labIA_Workflow.Tools.Mock
{
    public class EstoqueTool
    {
        [Description("Retorna caracteristicas do estoque de um produto.")]
        public string ConsultarEstoque([Description("Nome do produto, ex: mouse, teclado, monitor")] string produto)
        {
            var estoque = new Dictionary<string, int>
            {
                { "mouse", 10 },
                { "teclado", 5 },
                { "monitor", 0 }
            };

            if (estoque.TryGetValue(produto.ToLower(), out int quantidade))
                return $"O estoque do {produto} é: {quantidade}";

            return $"Produto {produto} não encontrado no estoque.";
        }

        [Description("Retorna o preço de um produto.")]
        public string ConsultarPreco(
            [Description("Nome do produto, ex: mouse, teclado, monitor")] string produto)
        {
            var precos = new Dictionary<string, double>
            {
                { "mouse", 19.99d },
                { "teclado", 29.99d },
                { "monitor", 1139.99d }
            };

            if (precos.TryGetValue(produto.ToLower(), out double preco))
                return $"O preço do {produto} é: R${preco}";

            return $"Produto {produto} não encontrado na lista de preços.";
        }
    }
}
