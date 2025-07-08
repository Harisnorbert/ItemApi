namespace ListManagerApi.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Price { get; set; }
    public int Quantity { get; set; }
}
