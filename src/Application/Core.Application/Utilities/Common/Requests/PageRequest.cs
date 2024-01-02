namespace Core.Application.Utilities.Common.Requests;

public class PageRequest
{
    /// <summary>
    /// Kaçıncı sayfa talep ediliyor. (1. sayfa, 0. index ile başlar)
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// Toplam kaç sayfa talep ediliyor.
    /// </summary>
    public int PageSize { get; set; }
}
