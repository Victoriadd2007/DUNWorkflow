
namespace DUNWorkflow.Models
{
    public class BaseCard
    {
        public string? CardType { get; set; }
        public string? Name { get; set; }
        public string? CardCode { get; set; }
        public string? CardImage { get; set; }

        public BaseCard() { }

        public BaseCard(string? cardType, string? name, string? cardCode, string? cardImage)
        {
            CardType = cardType;
            Name = name;
            CardCode = cardCode;
            CardImage = cardImage;
        }
    }

    public class CardManager
    {
        public List<BaseCard> CardList { get; set; } = new List<BaseCard>();
    }
}