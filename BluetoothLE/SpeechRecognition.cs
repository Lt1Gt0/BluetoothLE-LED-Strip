using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
//https://stackoverflow.com/questions/2853413/getting-data-from-a-microphone-in-c-sharp
public class SpeechRecognition {
    public static SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
    public static Grammar dictionaryGrammar = new DictationGrammar();
    //recognizer.LoadGrammar(dictionaryGrammar);
}
