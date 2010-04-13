package com.example.jndi;

import java.util.Hashtable;

import javax.naming.Context;
import javax.naming.NamingEnumeration;
import javax.naming.NamingException;
import javax.naming.directory.Attribute;
import javax.naming.directory.Attributes;
import javax.naming.directory.DirContext;
import javax.naming.directory.InitialDirContext;

import junit.framework.TestCase;

public class Demo extends TestCase {
	private DirContext getInitialDirectoryContext() throws NamingException {
		Hashtable<String, String> env = new Hashtable<String, String>();
		env.put(Context.INITIAL_CONTEXT_FACTORY, "com.sun.jndi.ldap.LdapCtxFactory");
		env.put(Context.PROVIDER_URL, "ldap://localhost:389/dc=example,dc=com");
		env.put(Context.OBJECT_FACTORIES, "com.example.jndi.ServiceFactory");
		env.put(Context.SECURITY_AUTHENTICATION, "simple");
		env.put(Context.SECURITY_PRINCIPAL, "cn=Directory Manager");
		env.put(Context.SECURITY_CREDENTIALS, "admin123");
		return new InitialDirContext(env);
	}
	
	public void testConnectAndGetAttributes() throws NamingException {
		DirContext ctx = getInitialDirectoryContext();
		Attributes attrs = ctx.getAttributes("");
		NamingEnumeration<? extends Attribute> e = attrs.getAll();
		while(e.hasMore()) {
			System.out.println(e.next());
		}
		ctx.close();
	}
	
	public void testAddService() throws NamingException {
		DirContext ctx = getInitialDirectoryContext();
		Service demoService = new Service(
				"{F6E978E7-A0BC-47ae-95A9-219CD40C5993}", 
				"demoService", 
				"http://localhost:20080/demo/");
		ctx.rebind("cn=demoService,o=Services", demoService);
		ctx.close();
	}

	public void testDeleteService() throws NamingException {
		DirContext ctx = getInitialDirectoryContext();
		ctx.unbind("cn=demoService,o=Services");
		ctx.close();
	}
	
	public void testServiceFactory() throws NamingException {
		DirContext ctx = getInitialDirectoryContext();
		Service demoService = (Service) ctx.lookup(
				"cn=demoService,o=Services");
		System.out.println(demoService);
	}
}
