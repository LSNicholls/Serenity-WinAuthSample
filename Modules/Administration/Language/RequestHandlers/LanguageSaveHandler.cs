using MyRow = WinAuthSample.Administration.LanguageRow;
using MyRequest = Serenity.Services.SaveRequest<WinAuthSample.Administration.LanguageRow>;
using MyResponse = Serenity.Services.SaveResponse;


namespace WinAuthSample.Administration;

public interface ILanguageSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> { }
public class LanguageSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, ILanguageSaveHandler
{
    public LanguageSaveHandler(IRequestContext context)
         : base(context)
    {
    }
}