package com.example.jndi;

import javax.naming.NameNotFoundException;
import javax.naming.NamingException;
import javax.naming.directory.Attribute;
import javax.naming.directory.Attributes;
import javax.naming.directory.BasicAttribute;
import javax.naming.directory.BasicAttributes;

public class Service extends UnimplementedDirContext {
	String _name;
    String _serviceUri;
    String _uid;
    
    public Service(String uid, String name, String uri) {    	
    	_uid = uid;
    	_name = name;
    	_serviceUri = uri;    	
    }
    
    public Service(Attributes inAttrs) {
    	_name = inAttrs.get("name").toString();
    	_serviceUri = inAttrs.get("serviceUri").toString();
    	_uid = inAttrs.get("uid").toString();
    }
    
    @Override
    public Attributes getAttributes(String name) throws NamingException {
    	if (! name.equals("")) {
    		throw new NameNotFoundException();
    	}

    	Attributes attrs = new BasicAttributes(true);  // Case ignore
    	Attribute oc = new BasicAttribute("objectclass");
    	oc.add("extensibleObject");
    	oc.add("top");
    	attrs.put(oc);
    	attrs.put("objectclass", "Service");
    	attrs.put("name", _name);
    	attrs.put("uid", _uid);
    	attrs.put("serviceUri", _serviceUri);
    	return attrs;
    }

    public String toString() {
    	return _name + " @ " + _serviceUri;
    }
}

