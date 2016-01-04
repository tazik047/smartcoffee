package com.smartqueue.zadorozhnii.stanislav.smartqueue;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Fragment;
import android.app.FragmentTransaction;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.ApiCaller;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.PostExecuteWorker;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.RequestType;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.CoffeeMachine;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.Order;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.QueueItem;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.SelectListItem;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Tazik on 03.01.2016.
 */
public class Main extends Fragment {

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_main, null);
    }
}
