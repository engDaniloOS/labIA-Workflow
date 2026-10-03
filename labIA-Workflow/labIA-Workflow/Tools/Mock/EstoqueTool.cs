using labIA_Workflow.Models;
using System.ComponentModel;

namespace labIA_Workflow.Tools.Mock
{
    public class EstoqueTool
    {
        [Description("Retorna caracteristicas do estoque de um produto.")]
        public string ConsultarEstoque([Description("Nome do produto, ex: mouse, teclado, monitor")] string produto)
        {
            var item = Estoque.GetProdutoByName(produto);
        
            if (item != null)
                return $"O estoque do {produto} é: {item.Quantidade}";

            return $"Produto {produto} não encontrado no estoque.";
        }

        [Description("Retorna o preço de um produto.")]
        public string ConsultarPreco(
            [Description("Nome do produto, ex: mouse, teclado, monitor")] string produto)
        {
            var item = Estoque.GetProdutoByName(produto);

            if (item != null)
                return $"O preço do {produto} é: R${item.Preco}";

            return $"Produto {produto} não encontrado na lista de preços.";
        }

        [Description("Retorna produtos com preço até o valor informado.")]
        public string ConsultarProdutosPorLimiteDePreco(
            [Description("Limite de preço")] double preco)
        {
            var produtos = Estoque.GetProdutoUpToPreco(preco);

            return string.Join(", ", produtos.Select(p => $"{p.Nome}: R${p.Preco}"));
        }
    }
}
