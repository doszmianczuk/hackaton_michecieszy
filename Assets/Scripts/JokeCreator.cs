using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class JokeCreator : MonoBehaviour
{
    public RectTransform textPanel;
    public TMP_Text outputText;
    public float hideAfterSeconds = 5f;
    private readonly string apiKey = "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";
    private readonly string modelName = "llama3-70b-8192";

    private bool isRequestInProgress = false;
    public void ShowText(string message, float duration)
    {
        outputText.text = message;
        textPanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TellJoke();
        }
    }
    
    public void TellJoke()
    {
        if (!isRequestInProgress)
        {
            _ = GetGroqResponse(
                "Tell me a short joke, don't tell it's a joke, make a new line between the question and the answer");
        }
    }

    public async Task GetGroqResponse(string userMessage)
    {
        isRequestInProgress = true;

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var requestData = new
            {
                messages = new[]
                {
                    new { role = "user", content = userMessage }
                },
                model = modelName
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GroqResponse>(jsonString);

                string reply = result.choices[0].message.content;
                Debug.Log(reply);

                if (outputText != null)
                {
                    ShowText(reply, hideAfterSeconds);
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                Debug.LogError($"ERROR: {error}");
                if (outputText != null)
                    ShowText($"ERROR: {error}", hideAfterSeconds);
            }
        }

        isRequestInProgress = false;
    }

    [System.Serializable]
    public class GroqResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    [System.Serializable]
    public class Message
    {
        public string content;
    }
}