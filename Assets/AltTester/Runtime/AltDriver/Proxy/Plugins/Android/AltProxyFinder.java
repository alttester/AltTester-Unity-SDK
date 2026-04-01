package com.alttester.utils;

import java.util.Iterator;
import java.util.List;
import java.net.InetSocketAddress;
import java.net.Proxy;
import java.net.ProxySelector;
import java.net.URI;

public class AltProxyFinder {
    public static String getProxy(String uri) {
        android.util.Log.d("AltProxyFinder", "getProxy called");
        try {
			System.setProperty("java.net.useSystemProxies", "true");

            List<Proxy> proxies = ProxySelector.getDefault().select(new URI(uri));
            android.util.Log.d("AltProxyFinder", "ProxySelector returned " + proxies.size() + " proxies");

            for (Iterator<Proxy> iter = proxies.iterator(); iter.hasNext(); ) {
                Proxy proxy = iter.next();
                InetSocketAddress address = (InetSocketAddress) proxy.address();

                if (address != null) {
                    android.util.Log.d("AltProxyFinder", "Proxy found: type=" + proxy.type());
                    return String.format("%s://%s:%s", proxy.type(), address.getHostName(), address.getPort());
                }
            }
        } catch (Exception e) {
            android.util.Log.e("AltProxyFinder", "getProxy exception: " + e.getMessage(), e);
            e.printStackTrace();
        }

        android.util.Log.d("AltProxyFinder", "No proxy found");
		return null;
    }
}
