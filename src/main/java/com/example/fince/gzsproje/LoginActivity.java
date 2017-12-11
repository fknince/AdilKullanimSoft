package com.example.fince.gzsproje;

import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Toast;

import com.facebook.AccessToken;
import com.facebook.AccessTokenTracker;
import com.facebook.CallbackManager;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.FacebookSdk;
import com.facebook.Profile;
import com.facebook.login.LoginResult;
import com.facebook.login.widget.LoginButton;

import java.io.Console;

public class LoginActivity extends AppCompatActivity {
    private LoginButton loginButton;
    private Profile profile;
    private FacebookController fc;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        FacebookSdk.sdkInitialize(getApplicationContext());
        setContentView(R.layout.activity_login);
        loginButton=(LoginButton)findViewById(R.id.fb_login);
        fc=new FacebookController(loginButton,getApplicationContext(),this);




    }

    @Override
    protected void onResume() {
        super.onResume();
        this.profile=fc.getCurrentProfile();
        fc.profileChanged(this.profile);

    }

    @Override
    protected void onPause() {
        super.onPause();

    }

    @Override
    protected void onStop() {
        super.onStop();
        fc.stopActions();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        fc.getCallbackManager().onActivityResult(requestCode,resultCode,data);
    }
    public void changeActivity(Intent intent)
    {
        startActivity(intent);
        finish();
    }
    public void incomingMesageController(String message)
    {

    }

}
