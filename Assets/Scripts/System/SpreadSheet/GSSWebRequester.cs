using System.Text;
using UnityEngine.Networking;

/// <summary>
/// �X�v���b�h�V�[�g�̃V�[�g�����󂯎��C���^�[�t�F�[�X
/// </summary>
public interface IGSSWebReceiver
{
    /// <summary>
    /// �󂯎�����ۂ̏���
    /// </summary>
    /// <param name="download"></param>
    void Receive(DownloadHandler download);
}

/// <summary>
/// �X�v���b�h�V�[�g�̃V�[�g�����擾����N���X
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
    /// ������
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
    /// �\��.
    /// ����
    /// </summary>
    public void Request()
    {
        var operation = _request.SendWebRequest();
        while (!operation.isDone) { }
        _receiver.Receive(_request.downloadHandler);
    }

    /// <summary>
    /// �\��.
    /// �񓯊�
    /// </summary>
    /// <returns></returns>
    public System.Collections.IEnumerator RequestAsync()
    {
        yield return _request.SendWebRequest();
        _receiver.Receive(_request.downloadHandler);
    }
}
