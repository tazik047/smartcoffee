package com.smartqueue.zadorozhnii.stanislav.smartqueue;

import android.app.Application;
import android.content.Context;

/**
 * Created by Tazik on 27.12.2015.
 */
public class MyApplication extends Application {

    private static Context appContext;
    public static Context getContext() {
        return appContext;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        appContext = this;
    }

}
