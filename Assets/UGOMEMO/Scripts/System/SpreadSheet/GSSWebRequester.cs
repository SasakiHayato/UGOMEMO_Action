using System.Text;
using UnityEngine.Networking;

/// <summary>
/// スプレッドシートのシート情報を受け取るインターフェース
/// </summary>
public interface IGSSWebReceiver
{
    /// <summary>
    /// 受け取った際の処理
    /// </summary>
    /// <param name="download"></param>
    void Receive(DownloadHandler download);
}

/// <summary>
/// スプレッドシートのシート情報を取得するクラス
/// </summary>
public class GSSWebRequester
{
    //============================================================================
    // constant
    //============================================================================

    private const string HTTP = "https://script.google.com/macros/s/";
    private const string DEPLOY_KEY = "AKfycbyGZxjO7acL6aJdOghbWblIFztsL_suIvzuwfO9GijTReyfAoi8PD7sc97WpE92vifV";
    private const string QUERY = "/exec";
    
    private const string GSS_KEY = "1GQGkF1ay2nWhLkMJ-SPAlVd57IRiY53Iz1oQFQ2jSp8";
    
    private readonly IGSSWebReceiver _receiver;

    private readonly UnityWebRequest _request;

    //============================================================================
    // constructor
    //============================================================================

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="query"></param>

    public GSSWebRequester(IGSSWebReceiver receiver, GSS_Sheet sheet_id)
    {
        _receiver = receiver;
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(HTTP);
        stringBuilder.Append(DEPLOY_KEY);
        stringBuilder.Append(QUERY);
        stringBuilder.Append($"?spreadsheetID={GSS_KEY}");
        stringBuilder.Append($"&sheetID={sheet_id}");

        _request = UnityWebRequest.Get(stringBuilder.ToString());
    }

    /// <summary>
    /// 申請.
    /// 同期
    /// </summary>
    public void Request()
    {
        var operation = _request.SendWebRequest();
        while (!operation.isDone) { }
        _receiver.Receive(_request.downloadHandler);
    }

    /// <summary>
    /// 申請.
    /// 非同期
    /// </summary>
    /// <returns></returns>
    public System.Collections.IEnumerator RequestAsync()
    {
        yield return _request.SendWebRequest();
        _receiver.Receive(_request.downloadHandler);
    }
}
