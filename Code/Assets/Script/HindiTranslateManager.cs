using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

// Unique TranslationRequest class for Google Translate
[System.Serializable]
public class GoogleTranslationRequest
{
    public string q;        // The text to translate
    public string source;   // Source language code (e.g., "hi" for Hindi)
    public string target;   // Target language code (e.g., "en" for English)
    public string format;   // Format of the text (e.g., "text")
}

// Unique TranslationResponse class for Google Translate
[System.Serializable]
public class GoogleTranslationResponse
{
    public Data data;

    [System.Serializable]
    public class Data
    {
        public Translation[] translations;
    }

    [System.Serializable]
    public class Translation
    {
        public string translatedText;
    }
}

public class HindiTranslateManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField hindiInputField;       // Assign in Inspector
    public TextMeshProUGUI translatedText;       // Assign in Inspector
    public Button translateButton;               // Assign in Inspector

    [Header("Google Translate API Settings")]
    public string apiKey = "YOUR_GOOGLE_API_KEY";  // Replace with your Google API key
    private string googleTranslateUrl = "https://translation.googleapis.com/language/translate/v2"; // Google Translate API endpoint

    void Start()
    {
        if (translateButton != null)
        {
            translateButton.onClick.AddListener(OnTranslateButtonClicked);
        }
        else
        {
            Debug.LogError("Translate Button is not assigned in the Inspector.");
        }
    }

    // Method called when Translate button is clicked
    void OnTranslateButtonClicked()
    {
        string inputText = hindiInputField.text.Trim();
        if (string.IsNullOrEmpty(inputText))
        {
            translatedText.text = "कृपया अनुवाद के लिए पाठ दर्ज करें।"; // "Please enter text to translate."
            return;
        }

        StartCoroutine(TranslateText(inputText, "hi", "en")); // Translating from Hindi to English
    }

    // Coroutine to handle the translation request
    IEnumerator TranslateText(string text, string sourceLang, string targetLang)
    {
        // Construct the request URL with parameters
        string url = $"{googleTranslateUrl}?key={apiKey}&q={UnityWebRequest.EscapeURL(text)}&source={sourceLang}&target={targetLang}&format=text";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                GoogleTranslationResponse response = JsonUtility.FromJson<GoogleTranslationResponse>(request.downloadHandler.text);
                if (response != null && response.data != null && response.data.translations.Length > 0)
                {
                    string translatedTextResult = response.data.translations[0].translatedText;
                    translatedText.text = translatedTextResult;
                }
                else
                {
                    translatedText.text = "अनुवाद विफल: रिक्त प्रतिक्रिया।"; // "Translation failed: Empty response."
                }
            }
            catch (System.Exception e)
            {
                translatedText.text = "अनुवाद त्रुटि: प्रतिक्रिया पार्स करने में विफल।"; // "Translation error: Failed to parse response."
                Debug.LogError($"JSON Parsing Error: {e.Message}");
            }
        }
        else
        {
            // Handle errors
            translatedText.text = $"त्रुटि: {request.error}"; // "Error: [error message]"
            Debug.LogError($"Translation API Error: {request.error}");
        }
    }
}

//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Networking;

//// Unique TranslationRequest class for Hindi to English
//[System.Serializable]
//public class HindiTranslationRequest
//{
//    public string q;        // The text to translate
//    public string source;   // Source language code (e.g., "hi" for Hindi)
//    public string target;   // Target language code (e.g., "en" for English)
//    public string format;   // Format of the text (e.g., "text")
//}

//// Unique TranslationResponse class for Hindi to English
//[System.Serializable]
//public class HindiTranslationResponse
//{
//    public Data data;

//    [System.Serializable]
//    public class Data
//    {
//        public Translation[] translations;
//    }

//    [System.Serializable]
//    public class Translation
//    {
//        public string translatedText;
//    }
//}

//public class HindiTranslateManager : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public TMP_InputField hindiInputField;       // Assign in Inspector
//    public TextMeshProUGUI translatedText;       // Assign in Inspector
//    public Button translateButton;               // Assign in Inspector

//    [Header("Translate API Settings")]
//    public string apiUrl = "https://libretranslate.de/translate"; // Argos Translate API endpoint

//    void Start()
//    {
//        if (translateButton != null)
//        {
//            translateButton.onClick.AddListener(OnTranslateButtonClicked);
//        }
//        else
//        {
//            Debug.LogError("Translate Button is not assigned in the Inspector.");
//        }
//    }

//    // Method called when Translate button is clicked
//    void OnTranslateButtonClicked()
//    {
//        string inputText = hindiInputField.text.Trim();
//        if (string.IsNullOrEmpty(inputText))
//        {
//            translatedText.text = "कृपया अनुवाद के लिए पाठ दर्ज करें।"; // "Please enter text to translate."
//            return;
//        }

//        StartCoroutine(TranslateText(inputText, "hi", "en"));
//    }

//    // Coroutine to handle the translation request
//    IEnumerator TranslateText(string text, string sourceLang, string targetLang)
//    {
//        // Manually construct JSON request with double quotes around keys
//        string jsonData = $"{{\"q\":\"{text}\", \"source\":\"{sourceLang}\", \"target\":\"{targetLang}\", \"format\":\"text\"}}";

//        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");

//        // Send the request and wait for response
//        yield return request.SendWebRequest();

//        if (request.result == UnityWebRequest.Result.Success)
//        {
//            try
//            {
//                HindiTranslationResponse response = JsonUtility.FromJson<HindiTranslationResponse>(request.downloadHandler.text);
//                if (response != null && response.data != null && response.data.translations.Length > 0)
//                {
//                    string translatedTextResult = response.data.translations[0].translatedText;
//                    translatedText.text = translatedTextResult;
//                }
//                else
//                {
//                    translatedText.text = "अनुवाद विफल: रिक्त प्रतिक्रिया।"; // "Translation failed: Empty response."
//                }
//            }
//            catch (System.Exception e)
//            {
//                translatedText.text = "अनुवाद त्रुटि: प्रतिक्रिया पार्स करने में विफल।"; // "Translation error: Failed to parse response."
//                Debug.LogError($"JSON Parsing Error: {e.Message}");
//            }
//        }
//        else
//        {
//            // Handle errors
//            translatedText.text = $"त्रुटि: {request.error}"; // "Error: [error message]"
//            Debug.LogError($"Translation API Error: {request.error}");
//        }
//    }
//}

//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Networking;

//// Unique TranslationRequest class for Hindi to English
//[System.Serializable]
//public class HindiTranslationRequest
//{
//    public string q;        // The text to translate
//    public string source;   // Source language code (e.g., "hi" for Hindi)
//    public string target;   // Target language code (e.g., "en" for English)
//    public string format;   // Format of the text (e.g., "text")
//}

//// Unique TranslationResponse class for Hindi to English
//[System.Serializable]
//public class HindiTranslationResponse
//{
//    public Data data;

//    [System.Serializable]
//    public class Data
//    {
//        public Translation[] translations;
//    }

//    [System.Serializable]
//    public class Translation
//    {
//        public string translatedText;
//    }
//}

//public class HindiTranslateManager : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public TMP_InputField hindiInputField;       // Assign in Inspector
//    public TextMeshProUGUI translatedText;       // Assign in Inspector
//    public Button translateButton;               // Assign in Inspector

//    [Header("LibreTranslate Settings")]
//    public string apiUrl = "https://libretranslate.com/translate"; // LibreTranslate API endpoint

//    void Start()
//    {
//        if (translateButton != null)
//        {
//            translateButton.onClick.AddListener(OnTranslateButtonClicked);
//        }
//        else
//        {
//            Debug.LogError("Translate Button is not assigned in the Inspector.");
//        }
//    }

//    // Method called when Translate button is clicked
//    void OnTranslateButtonClicked()
//    {
//        string inputText = hindiInputField.text.Trim();
//        if (string.IsNullOrEmpty(inputText))
//        {
//            translatedText.text = "कृपया अनुवाद के लिए पाठ दर्ज करें।"; // "Please enter text to translate."
//            return;
//        }

//        StartCoroutine(TranslateText(inputText, "hi", "en"));
//    }

//    // Coroutine to handle the translation request
//    IEnumerator TranslateText(string text, string sourceLang, string targetLang)
//    {
//        HindiTranslationRequest translationRequest = new HindiTranslationRequest
//        {
//            q = text,
//            source = sourceLang,
//            target = targetLang,
//            format = "text"
//        };

//        string jsonData = JsonUtility.ToJson(translationRequest); // Serialize using JsonUtility

//        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");

//        // Send the request and wait for response
//        yield return request.SendWebRequest();

//        if (request.result == UnityWebRequest.Result.Success)
//        {
//            try
//            {
//                HindiTranslationResponse response = JsonUtility.FromJson<HindiTranslationResponse>(request.downloadHandler.text);
//                if (response != null && response.data != null && response.data.translations.Length > 0)
//                {
//                    string translatedTextResult = response.data.translations[0].translatedText;
//                    translatedText.text = translatedTextResult;
//                }
//                else
//                {
//                    translatedText.text = "अनुवाद विफल: रिक्त प्रतिक्रिया।"; // "Translation failed: Empty response."
//                }
//            }
//            catch (System.Exception e)
//            {
//                translatedText.text = "अनुवाद त्रुटि: प्रतिक्रिया पार्स करने में विफल।"; // "Translation error: Failed to parse response."
//                Debug.LogError($"JSON Parsing Error: {e.Message}");
//            }
//        }
//        else
//        {
//            // Handle errors
//            translatedText.text = $"त्रुटि: {request.error}"; // "Error: [error message]"
//            Debug.LogError($"Translation API Error: {request.error}");
//        }
//    }
//}

