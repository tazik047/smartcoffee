package com.smartqueue.zadorozhnii.stanislav.smartqueue.logic;

import android.content.Entity;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.SimpleCursorAdapter;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.MyApplication;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.User;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.CookieStore;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpDelete;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.cookie.Cookie;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.cookie.BasicClientCookie;
import org.apache.http.message.BasicHttpResponse;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Field;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

/**
 * Created by Tazik on 27.12.2015.
 */
public class ApiCaller {

    private String _url;
    private RequestType _request;
    private PostExecuteWorker _postExecute;
    private HttpEntity _entity;
    private static Calendar expirationTime;
    private static String cookie;
    private static DataBaseHelper _db;
    private static String COOKIE_NAME = ".ASPXAUTH";
    //public static String DOMAIN = "10.23.13.137";
    //public static String DOMAIN = "192.168.43.147";
    public static String DOMAIN = "192.168.0.147";

    private String getCookie(){
        if(cookie==null){
            SQLiteDatabase db = getDatabase().getReadableDatabase();
            Cursor userCursor = db.rawQuery("select * from " + DataBaseHelper.TABLE, null);

            if(userCursor.getCount()==0){
                //TODO Redirect to login activity
                return null;
            }

            userCursor.moveToFirst();
            Calendar calendar = Calendar.getInstance();
            String mils = userCursor.getString(userCursor.getColumnIndex(DataBaseHelper.COLUMN_EXPIRATION_DATE));
            long l = Long.parseLong(mils);
            calendar.setTimeInMillis(l);
            Calendar now = getNow();
            if(calendar.compareTo(now)<=0){
                //TODO ask again cookies
                return null;
            }
            cookie = userCursor.getString(userCursor.getColumnIndex(DataBaseHelper.COLUMN_COOKIE));
            expirationTime = calendar;
            return cookie;
        }

        Calendar now = getNow();

        if(expirationTime.compareTo(now)<=0){
            //TODO ask again cookies yet
            cookie = null;
            return null;
        }

        return cookie;
    }

    private Calendar getNow(){
        Calendar now = Calendar.getInstance();
        now.setTimeInMillis(System.currentTimeMillis());
        return now;
    }

    private DataBaseHelper getDatabase(){
        if(_db == null){
            _db = new DataBaseHelper(MyApplication.getContext());
        }

        return _db;
    }

    public void authorize(User user, DefaultHttpClient client){
        CookieStore cookieStore = client.getCookieStore();
        for(Cookie c : cookieStore.getCookies()){
            if(c.getName().equals(COOKIE_NAME)){
                SQLiteDatabase db = getDatabase().getWritableDatabase();
                Gson g = new Gson();
                String insert = "INSERT INTO %s (%s, %s, %s) VALUES ('%s', '%s', '%d')";
                insert = String.format(insert, DataBaseHelper.TABLE, DataBaseHelper.COLUMN_USER, DataBaseHelper.COLUMN_COOKIE, DataBaseHelper.COLUMN_EXPIRATION_DATE,
                        g.toJson(user), c.getValue(), c.getExpiryDate().getTime());
                db.execSQL("DELETE FROM " + DataBaseHelper.TABLE);
                db.execSQL(insert);
                break;
            }
        }
    }

    public ApiCaller(String url, RequestType request, PostExecuteWorker postExecute){
        this(url, request, postExecute, null, null);
    }

    public ApiCaller(String url, RequestType request, PostExecuteWorker postExecute, Object param, HttpEntity entity) {
        _url = "http://"+DOMAIN+":4747/" + url;
        _request = request;
        _postExecute = postExecute;
        if (entity != null) {
            _entity = entity;
        } else if (param != null) {
            _entity = GetHttpEntity(param);
        }
    }

    protected HttpEntity GetHttpEntity(Object obj){
        List<BasicNameValuePair> nameValuePairs = new ArrayList<BasicNameValuePair>();
        for (Field f :obj.getClass().getFields()) {
            try {
                if(f.get(obj)!=null) {
                    nameValuePairs.add(new BasicNameValuePair(f.getName(), f.get(obj).toString()));
                }
            } catch (IllegalAccessException e) {
                return null;
            }
        }
        try {
            return new UrlEncodedFormEntity(nameValuePairs);
        } catch (UnsupportedEncodingException e) {
            return null;
        }
    }

    protected HttpUriRequest GetRequest(){
        switch (_request){
            case Get:
                return new HttpGet(_url);
            case Post:
                HttpPost post = new HttpPost(_url);
                post.setEntity(_entity);
                return post;
            case Put:
                HttpPut put =  new HttpPut(_url);
                put.setEntity(_entity);
                return put;
            case Delete:
                return new HttpDelete(_url);
        }
        return null;
    }

    public void execute() {
        HttpClient client = new DefaultHttpClient();
        if(getCookie()!=null) {
            CookieStore cookieStore = ((DefaultHttpClient) client).getCookieStore();
            BasicClientCookie c = new BasicClientCookie(COOKIE_NAME, getCookie());
            c.setDomain(DOMAIN);
            c.setPath("/");
            cookieStore.addCookie(c);
        }
        HttpUriRequest request = GetRequest();
        Object result = new Object();

        HttpResponse response = null;

        try {
            response = client.execute(request);
            /*if(response.getStatusLine().getStatusCode()==200) {
                result = EntityUtils.toString(response.getEntity());
            }
            else{
                result = Integer.toString(response.getStatusLine().getStatusCode());
            }*/
        } catch (Exception e) {
            e.printStackTrace();
            Log.d("api", e.getMessage());
            return;
        }

        _postExecute.execute(this, client, response);
    }
}