package com.smartqueue.zadorozhnii.stanislav.smartqueue;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Fragment;
import android.app.FragmentTransaction;
import android.content.Intent;
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
import android.widget.ListView;
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
import org.w3c.dom.Text;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutionException;

/**
 * Created by Tazik on 03.01.2016.
 */
public class Queue extends Fragment {

    private View mProgressView;
    private View mMainFormView;

    private Spinner drinkSize;
    private Spinner drinkType;
    private EditText sugar;
    private Spinner coffeeMachine;

    private TextView timeToEnd;
    private LinearLayout queueList;

    private LoadPreferencesTask loadTask;
    private AddToQueueTask saveTask;
    private IsUserInQueueTask isInQueue;
    private GetFromQueueTask fromQueue;

    private int main_layout;

    public Queue(int layoutId){
        main_layout = layoutId;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        if(main_layout==R.layout.fragment_add_to_queue) {
            return startAddInQueue(inflater);
        }
        else{
            return startQueue(inflater);
        }
    }

    private View startQueue(LayoutInflater inflater){
        View view = inflater.inflate(R.layout.fragment_queue, null);
        mProgressView = view.findViewById(R.id.login_progress);
        mMainFormView = view.findViewById(R.id.main_layout);
        isInQueue = new IsUserInQueueTask();
        isInQueue.execute((Void) null);

        timeToEnd = (TextView)view.findViewById(R.id.timeToEnd);
        queueList = (LinearLayout)view.findViewById(R.id.queueList);

        fromQueue = new GetFromQueueTask();
        fromQueue.execute((Void) null);
        return view;
    }

    private View startAddInQueue(LayoutInflater inflater){
        View view = inflater.inflate(R.layout.fragment_add_to_queue, null);
        mProgressView = view.findViewById(R.id.login_progress);
        mMainFormView = view.findViewById(R.id.main_layout);
        isInQueue = new IsUserInQueueTask();
        isInQueue.execute((Void) null);

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
                saveTask = new AddToQueueTask(String.valueOf(sugarCount));
                saveTask.execute((Void) null);
            }
        });

        showProgress(true);
        loadTask = new LoadPreferencesTask();
        loadTask.execute((Void) null);
        return view;
    }

    private void reload(int layout){
        FragmentTransaction ft = getActivity().getFragmentManager().beginTransaction();
        ft.replace(R.id.fragmLayout, new Queue(layout));
        ft.commit();
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

    public class AddToQueueTask extends AsyncTask<Void, Void, Boolean> {

        private String sugarText;

        AddToQueueTask(String sugar){
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

            ApiCaller caller = new ApiCaller("api/queue", RequestType.Post,
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
            saveTask = null;
            showProgress(false);
            Toast.makeText(MyApplication.getContext(), "Заказ принят", Toast.LENGTH_LONG).show();
            reload(R.layout.fragment_queue);
        }

        @Override
        protected void onCancelled() {
            saveTask = null;
            showProgress(false);
        }
    }

    public class IsUserInQueueTask extends AsyncTask<Void, Void, Boolean> {

        @Override
        protected Boolean doInBackground(Void... params) {
            final boolean result[] = new boolean[1];

            ApiCaller caller = new ApiCaller("api/queue", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if(response.getStatusLine().getStatusCode()==404){
                                result[0] = false;
                            }
                            else{
                                result[0] = true;
                            }
                        }
                    });
            caller.execute();

            return result[0];
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            isInQueue = null;
            if(success){
                if(main_layout!=R.layout.fragment_queue){
                    reload(R.layout.fragment_queue);
                }
            }
            else{
                if(main_layout!=R.layout.fragment_add_to_queue){
                    reload(R.layout.fragment_add_to_queue);
                }
            }
        }

        @Override
        protected void onCancelled() {
            isInQueue = null;
            showProgress(false);
        }
    }

    public class GetFromQueueTask extends AsyncTask<Void, Void, Boolean> {

        private QueueItem previous;
        final boolean result[] = new boolean[1];

        @Override
        protected Boolean doInBackground(Void... params) {



            while (!result[0]) {
                ApiCaller caller = new ApiCaller("api/queue", RequestType.Get,
                    new PostExecuteWorker() {
                        @Override
                        public void execute(final ApiCaller caller, final HttpClient client, final HttpResponse response) {
                            if (response.getStatusLine().getStatusCode() == 404) {
                                result[0] = true;
                            } else {
                                Gson g = new Gson();
                                Type t = new TypeToken<QueueItem>() {
                                }.getType();
                                QueueItem temp = null;
                                try {
                                    temp = g.fromJson(EntityUtils.toString(response.getEntity()), t);
                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                                final QueueItem item = temp;
                                getActivity().runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        timeToEnd.setText(item.timeToEnd);
                                        if(!item.equals(previous)) {
                                            item.GenerateList(queueList);
                                            previous = item;
                                        }
                                    }
                                });
                            }
                        }
                    });
                caller.execute();
                try {
                    Thread.sleep(1000, 0);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            return true;
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            fromQueue = null;
            reload(R.layout.fragment_add_to_queue);
        }

        @Override
        protected void onCancelled() {
            fromQueue = null;
            result[0] = true;
        }
    }
}
