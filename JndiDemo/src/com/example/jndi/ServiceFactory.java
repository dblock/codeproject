package com.example.jndi;

import javax.naming.*;
import javax.naming.directory.*;
import javax.naming.spi.DirObjectFactory;
import java.util.Hashtable;

public class ServiceFactory implements DirObjectFactory {
    
	public ServiceFactory() {
    	
    }

    public Object getObjectInstance(Object obj, Name name, Context ctx, 
    		Hashtable<?, ?> env, Attributes inAttrs) throws Exception {

    	if (obj instanceof DirContext) {
    		Attribute objectClass = inAttrs.get("objectClass");
    		NamingEnumeration<?> ne = objectClass.getAll();
    		while(ne.hasMore()) {
    			if (ne.next().equals("Service")) {
        			return new Service(inAttrs);    				
    			}
    		}
    	}
    	
    	return null;
	}

	public Object getObjectInstance(Object obj, Name name, Context ctx, Hashtable<?, ?> env) throws Exception {
	    return getObjectInstance(obj, name, ctx, env, null);
    }
}
