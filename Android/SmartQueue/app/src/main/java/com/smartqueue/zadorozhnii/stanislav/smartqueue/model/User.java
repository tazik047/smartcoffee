package com.smartqueue.zadorozhnii.stanislav.smartqueue.model;

/**
 * Created by Tazik on 27.12.2015.
 */
public class User {

    public long id;

    public String email;

    public String login;

    public String password;

    public boolean rememberMe;

    @Override
    public String toString() {
        return String.format("login=%s&password=%s&rememberMe=%s",login, password, rememberMe);
    }
}
