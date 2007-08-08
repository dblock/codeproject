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
            // bug: doesn't work for RadioButtonList
            if (control.type == 'checkbox' || control.type == 'radio') {
                var boolvalue = (value == "true" ? true : false);
                if(control.checked != boolvalue) {
                    return true;
                }
            } else if (control.type == 'select-one') {
                if(control.selectedIndex != value) {
                    return true;
                }
            } else {
                if(control.value != value) {
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
