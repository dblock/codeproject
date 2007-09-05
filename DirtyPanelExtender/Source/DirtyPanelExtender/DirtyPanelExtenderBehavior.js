Type.registerNamespace('DirtyPanelExtender');

DirtyPanelExtender.DirtyPanelExtenderBehavior = function(element) {

    DirtyPanelExtender.DirtyPanelExtenderBehavior.initializeBase(this, [element]);
    this._OnLeaveMessageValue = null;
}

DirtyPanelExtender.DirtyPanelExtenderBehavior.prototype = {

    isDirty : function() {
        var values_control = document.getElementById(this.get_element().id + "_Values");
        var values = values_control["value"].split(",");
        for (i in values) {
            var namevalue = values[i];
            var namevaluepair = namevalue.split(":");
            var name = namevaluepair[0];
            var value = (namevaluepair.length > 1 ? namevaluepair[1] : "");
            var control = document.getElementById(name);
            if (control == null) continue;
            // alert(control.id + " -> " + control.type);
            if (control.type == 'checkbox' || control.type == 'radio') {
                var boolvalue = (value == "true" ? true : false);
                if(control.checked != boolvalue) {
                    // alert("checkbox changed: " + control.checked + " vs. " + boolvalue);
                    return true;
                }
            } else if (control.type == 'select-one' || control.type == 'select-multiple') {
               if (namevaluepair.length > 2) {
                   if ( control.options.length > 0) {
                       // control is listbox
                       // there's data:value and selection:value
                       var code = value;
                       value = (namevaluepair.length > 2 ? namevaluepair[2] : "");
                       var optionValues = "";
                       // concat all listbox items
                       for( var cnt = 0; cnt < control.options.length; cnt++) {
                          if (code == 'data') {
                              optionValues += control.options[cnt].text;
                          } else if (code == 'selection') {
                              optionValues += control.options[cnt].selected;
                          }
                          optionValues += "\r\n";
                       }
                       if( encodeURIComponent(optionValues) != value ) {
                          // items in the listbox have changed
                          // alert("listbox " + code + " changed: " + encodeURIComponent(optionValues) + " vs. " + value);
                          return true;
                       }
                   }
               } else if(control.selectedIndex != value) {
                   // alert("dropdown selection changed: " + control.selectedIndex + " vs. " + value);
                   return true;
               }
            } else {
                if(encodeURIComponent(control.value) != value) {
                    // alert("control " + control.type + " changed: " + control.value + " vs. " + value);
                    return true;
                }
            }
        }
        return false;
    },
    
    initialize : function() {        
        DirtyPanelExtender.DirtyPanelExtenderBehavior.callBaseMethod(this, 'initialize');
        DirtyPanelExtender_dirtypanels[DirtyPanelExtender_dirtypanels.length] = this;
    },

    dispose : function() {
        DirtyPanelExtender.DirtyPanelExtenderBehavior.callBaseMethod(this, 'dispose');
    },

    get_OnLeaveMessage : function() {
        return this._OnLeaveMessageValue;
    },

    set_OnLeaveMessage : function(value) {
        this._OnLeaveMessageValue = value;
    }
}

DirtyPanelExtender.DirtyPanelExtenderBehavior.registerClass('DirtyPanelExtender.DirtyPanelExtenderBehavior', AjaxControlToolkit.BehaviorBase);

var DirtyPanelExtender_dirtypanels = new Array()

function DirtyPanelExtender_SuppressDirtyCheck()
{
  window.onbeforeunload = null;
}

function __newDoPostBack(eventTarget, eventArgument) 
{ 
  // supress prompting on postback
  DirtyPanelExtender_SuppressDirtyCheck();
  return __savedDoPostBack (eventTarget, eventArgument);
}

var __savedDoPostBack = __doPostBack;
__doPostBack = __newDoPostBack; 

window.onbeforeunload = function (eventargs) 
{
    for (i in DirtyPanelExtender_dirtypanels)
    {
        var panel = DirtyPanelExtender_dirtypanels[i];
        if (panel.isDirty()) 
        {
          if(! eventargs) eventargs = window.event;
          eventargs.returnValue = panel.get_OnLeaveMessage();
          break;
        }
    }
}
