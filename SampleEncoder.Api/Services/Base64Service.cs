using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleEncoder.Api.Models;

public interface IBase64Service
{
    Task<string> EncodeToBase64Async(string text);
    Task<IEnumerable<ResponseItem>> GetBase64StreamAsync();
    void CancelEncoding();
}

public class Base64Service : IBase64Service
{
    private readonly object lockObject = new object();
    private List<ResponseItem> responseItems;
    private bool isEncoding;

    public async Task<string> EncodeToBase64Async(string text)
    {
        lock (lockObject)
        {
            if (isEncoding)
            {
                throw new InvalidOperationException("Another encoding process is already in progress.");
            }
        }

        responseItems = new List<ResponseItem>();
        isEncoding = true;

        try
        {
            string convertedInputText = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));

            foreach (char character in convertedInputText)
            {
                responseItems.Add(new ResponseItem { Character = character.ToString() });
            }

            return convertedInputText;
        }
        finally
        {
            lock (lockObject)
            {
                isEncoding = false;
            }
        }
    }

    public async Task<IEnumerable<ResponseItem>> GetBase64StreamAsync()
    {
        return responseItems;
    }

    public void CancelEncoding()
    {
        lock (lockObject)
        {
            isEncoding = false;
        }
    }
}
