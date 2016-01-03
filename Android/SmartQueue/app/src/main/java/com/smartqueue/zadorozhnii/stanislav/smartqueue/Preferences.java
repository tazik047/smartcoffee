package com.smartqueue.zadorozhnii.stanislav.smartqueue;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Activity;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.infrastructure.AccountHelper;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.ApiCaller;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.LinkedHashMapAdapter;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.PostExecuteWorker;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.RequestType;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.CoffeeMachine;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.Order;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.SelectListItem;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.User;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;


public class Preferences extends Fragment {

    private View mProgressView;
    private View mMainFormView;

    private Spinner drinkSize;
    private Spinner drinkType;
    private EditText sugar;
    private Spinner coffeeMachine;

    private LoadPreferencesTask loadTask;
    private SavePreferencesTask saveTask;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_preferences, null);
        mProgressView = view.findViewById(R.id.login_progress);
        mMainFormView = view.findViewById(R.id.main_layout);

        drinkSize = (Spinner)view.findViewById(R.id.drinkSize);
        drinkType = (Spinner)view.findViewById(R.id.drinkType);
        sugar = (EditText)view.findViewById(R.id.sugar);
        coffeeMachine = (Spinner)view.findViewById(R.id.coffeeMachine);

        Button bt = (Button)view.findViewById(R.id.button);
        bt.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String t = sugar.getText().toString();
                int sugarCount = Integer.parseInt(t);
                if(sugarCount>10||sugarCount<0){
                    sugar.setError("Количество ложек должно быть в пределах от 0 до 10");
                }
                SelectListItem item = (SelectListItem)coffeeMachine.getSelectedItem();
                if(item.value.equals("")){
                    ((TextView)coffeeMachine.getSelectedView()).setError("Выберите кофеварку");
                    return;
                }
                showProgress(true);
                saveTask = new SavePreferencesTask(String.valueOf(sugarCount));
                saveTask.execute((Void) null);
            }
        });

        showProgress(true);
        loadTask = new LoadPreferencesTask();
        loadTask.execute((Void) null);
        return view;
    }

    /**
     * Shows the progress UI and hides the login form.
     */
    @TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
    private void showProgress(final boolean show) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = getResources().getInteger(android.R.integer.config_shortAnimTime);

            mMainFormView.setVisibility(show ? View.GONE : View.VISIBLE);
            mMainFormView.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mMainFormView.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
            mProgressView.animate().setDuration(shortAnimTime).alpha(
                    show ? 1 : 0).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
                }
            });
        } else {
            // The ViewPropertyAnimator APIs are not available, so simply show
            // and hide the relevant UI components.
            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
            mMainFormView.setVisibility(show ? View.GONE : View.VISIBLE);
        }
    }

    public class LoadPreferencesTask extends AsyncTask<Void, Void, Boolean> {

        @Override
        protected Boolean doInBackground(Void... params) {

            final Order[] preferences = {null};

            ApiCaller caller = new ApiCaller("api/preferences", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if (response.getStatusLine().getStatusCode() == 200) {
                                Gson g = new Gson();
                                Type t = new TypeToken<Order>() {
                                }.getType();
                                try {
                                    preferences[0] = g.fromJson(EntityUtils.toString(response.getEntity()), t);
                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                            }
                        }
                    });
            caller.execute();

            getActivity().runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    sugar.setText(preferences[0].sugar);
                }
            });


            caller = new ApiCaller("api/drinksizes", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if (response.getStatusLine().getStatusCode() == 200) {
                                Gson g = new Gson();
                                Type t = new TypeToken<ArrayList<SelectListItem>>() {
                                }.getType();
                                List<SelectListItem> items = null;
                                try {
                                    items = g.fromJson(EntityUtils.toString(response.getEntity()), t);
                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                                ArrayAdapter<SelectListItem> adapter = new ArrayAdapter<SelectListItem>( MyApplication.getContext(),
                                        R.layout.spinner_item, items);
                                adapter.setDropDownViewResource(R.layout.spinner_item);
                                drinkSize.setAdapter(adapter);
                                SelectListItem current = null;
                                for(SelectListItem i : items){
                                    if(preferences[0].size.equals(i.value)){
                                        current = i;
                                        break;
                                    }
                                }
                                int position = adapter.getPosition(current);
                                drinkSize.setSelection(position);
                            }
                        }
                    });
            caller.execute();

            caller = new ApiCaller("api/drinktypes", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if (response.getStatusLine().getStatusCode() == 200) {
                                Gson g = new Gson();
                                Type t = new TypeToken<ArrayList<SelectListItem>>() {
                                }.getType();
                                List<SelectListItem> items = null;
                                try {
                                    items = g.fromJson(EntityUtils.toString(response.getEntity()), t);
                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                                ArrayAdapter<SelectListItem> adapter = new ArrayAdapter<SelectListItem>( MyApplication.getContext(),
                                        R.layout.spinner_item, items);
                                adapter.setDropDownViewResource(R.layout.spinner_item);
                                drinkType.setAdapter(adapter);
                                SelectListItem current = null;
                                for(SelectListItem i : items){
                                    if(preferences[0].drink.equals(i.value)){
                                        current = i;
                                        break;
                                    }
                                }
                                int position = adapter.getPosition(current);
                                drinkType.setSelection(position);
                            }
                        }
                    });
            caller.execute();

            caller = new ApiCaller("api/coffeemachine", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if (response.getStatusLine().getStatusCode() == 200) {
                                Gson g = new Gson();
                                Type t = new TypeToken<ArrayList<CoffeeMachine>>() {
                                }.getType();
                                List<CoffeeMachine> machines = null;
                                try {
                                    machines = g.fromJson(EntityUtils.toString(response.getEntity()), t);
                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                                List<SelectListItem> items = new ArrayList<>();
                                SelectListItem defaultItem = new SelectListItem();
                                defaultItem.text = "Выберить кофеварку";
                                defaultItem.value = "";
                                items.add(defaultItem);
                                SelectListItem current = defaultItem;
                                for(CoffeeMachine i: machines){
                                    SelectListItem item = new SelectListItem();
                                    item.text = i.name;
                                    item.value = String.valueOf(i.id);
                                    items.add(item);
                                    if(item.value.equals(preferences[0].coffeeMachineId)){
                                        current = item;
                                    }
                                }
                                ArrayAdapter<SelectListItem> adapter = new ArrayAdapter<SelectListItem>( MyApplication.getContext(),
                                        R.layout.spinner_item, items);
                                adapter.setDropDownViewResource(R.layout.spinner_item);
                                coffeeMachine.setAdapter(adapter);
                                int position = adapter.getPosition(current);
                                coffeeMachine.setSelection(position);
                            }
                        }
                    });
            caller.execute();

            return true;
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            loadTask = null;
            showProgress(false);
        }

        @Override
        protected void onCancelled() {
            loadTask = null;
            showProgress(false);
        }
    }

    public class SavePreferencesTask extends AsyncTask<Void, Void, Boolean> {

        private String sugarText;

        SavePreferencesTask(String sugar){
            sugarText = sugar;
        }


        private String getValueFromSpinner(Spinner spinner){
            SelectListItem item = (SelectListItem)spinner.getSelectedItem();
            return item.value;
        }


        @Override
        protected Boolean doInBackground(Void... params) {

            final Order[] preferences = {null};

            preferences[0] = new Order();

            preferences[0].drink = getValueFromSpinner(drinkType);
            preferences[0].size = getValueFromSpinner(drinkSize);
            preferences[0].coffeeMachineId = getValueFromSpinner(coffeeMachine);
            preferences[0].sugar = sugarText;

            ApiCaller caller = new ApiCaller("api/preferences", RequestType.Post,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {

                        }
                    }, preferences[0], null);
            caller.execute();

            return true;
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            loadTask = null;
            showProgress(false);
            Toast.makeText(MyApplication.getContext(), "Данные успешно сохранены", Toast.LENGTH_LONG).show();
        }

        @Override
        protected void onCancelled() {
            loadTask = null;
            showProgress(false);
        }
    }
}


//String name = spinner.getSelectedItem().toString();
//String id = spinnerMap.get(name);