package com.example.fince.gzsproje;

import android.app.Activity;
import android.content.Intent;
import android.speech.RecognizerIntent;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.util.Log;
import android.widget.Toast;

import java.util.Locale;

/**
 * Created by fince on 24.11.2017.
 */

public class SpeechingController {
    TextToSpeech textToSpeech;
    Activity act;
    private String metin;
    private boolean isSpeaking;
    public SpeechingController(Activity act)
    {
        this.act=act;
    }
    public TextToSpeech text2Speech(String metin)
    {
        this.metin=metin;
        textToSpeech=new TextToSpeech(act, new TextToSpeech.OnInitListener() {
            @Override
            public void onInit(int i) {
                if (i == TextToSpeech.SUCCESS) {


                    int	result = textToSpeech.setLanguage(Locale.getDefault());



                    if (result == TextToSpeech.LANG_MISSING_DATA
                            || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                        Log.e("TTS", "This Language is not supported");
                    } else {

                        speakOut();
                    }

                } else {
                    Log.e("TTS", "Initilization Failed!");
                }

            }

        });
        return textToSpeech;

    }
    public void speakOut()
    {
        textToSpeech.speak(metin,TextToSpeech.QUEUE_FLUSH,null);

    }
    public void Speech2Text()
    {
        Intent intent=new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,Locale.getDefault());

        if(intent.resolveActivity(act.getPackageManager()) != null)
        {
            act.startActivityForResult(intent,10);
        }
        else
        {
            Toast.makeText(act.getBaseContext(),"Telefonunuz ne yazıkki ses-metin dönüşümünü desteklemiyor.",Toast.LENGTH_SHORT);
        }



    }
    public boolean isSpeaking() {
        return isSpeaking;
    }



}
