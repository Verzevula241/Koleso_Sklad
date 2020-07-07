<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Brig.aspx.cs" Inherits="Koleso.Brig" %>

<%@ Import Namespace="System.Collections.Generic" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Задачи</title>

    <link href="/resources/css/examples.css" rel="stylesheet" />
    <script>
        function resetFields(Panel) {
            Panel.items.each(function (f) { if (f.setValue) { f.setValue(''); f.clearInvalid(); } });
        };

        function telega(A, B, text) {
            if (A.charAt(0) == 'A' && B.charAt(0) == 'Б' || A.charAt(0) == 'Б' && B.charAt(0) == 'A') { text.setValue('Телега 1') }
            if (A.charAt(0) == 'A' && B.charAt(0) == 'В' || A.charAt(0) == 'В' && B.charAt(0) == 'A') { text.setValue('Телега 2') }
            if (A.charAt(0) == 'Б' && B.charAt(0) == 'В' || A.charAt(0) == 'Б' && B.charAt(0) == 'В') { text.setValue('Телега 2') }
            if (A.charAt(0) == 'Д' && B.charAt(0) == 'Г' || A.charAt(0) == 'Г' && B.charAt(0) == 'Д') { text.setValue('Телега 2') }

        };


        var submitValue = function (grid, hiddenFormat, format) {
            hiddenFormat.setValue(format);
            grid.submitData(false, { isUpload: true });
        };



        function test() {

            var a = [];
            Ext.getCmp('CompanyInfoTab').items.items.forEach(element => {
                if (!element.hidden && element.xtype != 'netlabel') {
                    if (element.value) { a.push(true) } else { a.push(false) }
                }
            });
            gag = a.find(item => item == false);
            if (gag == false) { return false } else { return true };
        };

        function GetField(Grid) {
            var ext = Ext.encode(Grid.getRowsValues({ selectedOnly: true }));
            return ext;

        };


        function PilaProv() {

            var sel = App.Grid1.getSelection();
            if (sel[0].data.STATUS == 2) {
                Ext.Msg.show({
                    title: 'Подтверждение',
                    msg: 'Вы пытаетесь подтвердить запись. Вы уверены?',
                    buttons: Ext.Msg.YESNO,
                    fn: function (btn, text) {
                        if (btn == 'yes') {
                            debugger;
                            var name = sel[0].data.NAME;
                            var Bpart = sel[0].data.B;
                            switch (name) {
                                case 'перемещение на ПК':
                                    App.direct.DoYes(sel[0].data.ID, 3, {
                                        success: function (result) {
                                            App.direct.Log_up(sel[0].data.ID);
                                            App.direct.AfterYes(sel[0].data.ID, '5', 0);
                                            App.Grid1.getStore().reload()
                                        }

                                    });

                                    break;
                                case 'ПО складу':
                                    App.direct.DoYes(sel[0].data.ID, 3, {
                                        success: function (result) {
                                            App.direct.Log_up(sel[0].data.ID);

                                            App.direct.AfterYes(sel[0].data.ID, '4', 0);
                                            App.Grid1.getStore().reload()
                                        }
                                    });
                                    break;
                                case 'Перемещение':
                                    App.direct.DoYes(sel[0].data.ID, 3, {
                                        success: function (result) {

                                            App.direct.Log_up(sel[0].data.ID);
                                            App.Grid1.getStore().reload()
                                        }
                                    });
                                    break;
                                case 'С ПК на Склад':
                                    App.direct.DoYes(sel[0].data.ID, 3, {
                                        success: function (result) {
                                            App.direct.Log_up(sel[0].data.ID);
                                            App.direct.AfterYes(sel[0].data.ID, '4', 0);
                                            App.Grid1.getStore().reload()
                                        }
                                    });
                                    break;
                                case 'РМ-8 в нагревательную печь №1':
                                    App.direct.DoYes(sel[0].data.ID, 3, {
                                        success: function (result) {

                                            App.direct.Log_up(sel[0].data.ID);
                                            App.Grid1.getStore().reload()
                                        }
                                    });
                                    break;

                            }
                        }
                    },
                    icon: Ext.MessageBox.QUESTION
                });
            }
            else WarningMessageBox('Задача еще не выполнена');

        };

        function prov() {

            if (Ext.getCmp('Prolet').getValue() == null || Ext.getCmp('Prolet1').getValue() == null || Ext.getCmp('PlaceSklad').getValue() == null || Ext.getCmp('PlaceSklad1').getValue() == null || Ext.getCmp('KranMan').getValue() == null || Ext.getCmp('KranMan1').getValue() == null) {

            } else {
                telega(Ext.getCmp('PlaceSklad').getValue(), Ext.getCmp('PlaceSklad1').getValue(), Ext.getCmp('Telega'));

            }
        }

        function WarningMessageBox(message) {
            Ext.Msg.info({ ui: 'warning', title: 'Внимание!', html: message, iconCls: '#Error' });
        }
        function WarningMessageBox400(message) {
            Ext.Msg.info({ ui: 'warning', title: 'Внимание!', width: 400, html: message, iconCls: '#Error' });
        }
        function DangerMessageBox(message) {
            Ext.Msg.info({ ui: 'danger', title: 'Внимание!', html: message, iconCls: '#Exclamation' });
        }
        function SuccessMessageBox(message) {
            Ext.Msg.info({ ui: 'success', title: 'Внимание!', html: message, iconCls: '#Accept' });
        }
        function InfoMessageBox(message) {
            Ext.Msg.info({ ui: 'info', title: 'Внимание!', html: message, iconCls: '#Information' });
        }

    </script>
    <style>
        .x-btn-default-small {
            border-radius: 20px;
            padding-top: 4px;
            padding-right: 5px;
            width: 150px;
            height: 50px;
        }
    </style>
    <ext:XScript runat="server">
        <script>

            var saveData = function () {
                var data = #{ LogGrid }.getRowsValues({
                    visibleOnly: true,
                    excludeId: true
                }),
                    columns = #{ LogGrid }.getColumns.config,
                        headers = {
                        NAME: "Наименование",
                        A: "Склад",
                        B: "Место",
                        KRAN: "Выполнял",
                        BRIG: "Проверял",
                        DATE_COM: "Создана",
                        DATE_OUT: "Выполнена"

                        },
                        dataWithHeaders = [],
                        recordWithHeaders;

                Ext.each(columns, function (c) {
                    headers[c.dataIndex] = c.header.replace(/ /g, "_");
                });

                Ext.each(data, function (record) {
                    recordWithHeaders = {};
                    debugger;
                    for (var field in record) {
                        
                        recordWithHeaders[headers[field]] = record[field];
                    }
                    dataWithHeaders.push(recordWithHeaders);
                });
  
                #{ GridData }.setValue(Ext.encode(dataWithHeaders));
            };



            var applyFilter = function (field) {
                var store = #{ Grid1 }.getStore();
                store.filterBy(getRecordFilter());
            };

            var LogFilter = function (field) {
                var store = #{ LogGrid }.getStore();
                store.filterBy(LogRec());
            };

            var clearFilter = function () {
                #{ ComboBox1 }.reset();
                #{ PriceFilter }.reset();
                #{ ChangeFilter }.reset();
                #{ PctChangeFilter }.reset();
                #{ LastChangeFilter }.reset();

                #{ Store1 }.clearFilter();
            }

            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);

                if (typeof val != "string") {
                    return value.length == 0;
                }

                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            };

            var filterNumber = function (value, dataIndex, record) {
                var val = record.get(dataIndex);

                if (!Ext.isEmpty(value, false) && val != value) {
                    return false;
                }

                return true;
            };
            var filterDate = function (value, dataIndex, record) {
                debugger;

                var a = new Date(record.get(dataIndex).split('T')[0]);
                var val = Ext.Date.clearTime(a, true).getTime();

                if (!Ext.isEmpty(value, false) && val != Ext.Date.clearTime(value, true).getTime()) {
                    return false;
                }
                return true;
            };

            var LogRec = function () {

                var f = [];

                f.push({
                    filter: function (record) {
                        return filterDate(#{ DateField1 }.getValue() || "", "DATE_COM", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ LogName }.getValue() || "", "NAME", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ LogKran }.getValue() || "", "KRAN", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ LogA }.getValue() || "", "A", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ LogB }.getValue() || "", "B", record);
                    }
                });
                var len = f.length;

                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };

            };


            var getRecordFilter = function () {
                var f = [];

                f.push({
                    filter: function (record) {
                        return filterString(#{ NameBox }.getValue() || "", "NAME", record);
                    }
                });

                f.push({
                    filter: function (record) {
                        return filterString(#{ AFilter }.getValue() || "", "A", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ BFilter }.getValue() || "", "B", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ ProletBox }.getValue() || "", "KRAN", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ KranBox }.getValue() || "", "KRAN_MAC", record);
                    }
                });
                f.push({
                    filter: function (record) {
                        return filterString(#{ StatusBox }.getValue() || "", "STAT", record);
                    }
                });

                var len = f.length;

                return function (record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            };

            var handlePageSizeSelect = function (item, records) {
                var curPageSize = #{ LogGrid }.store.pageSize,
                    wantedPageSize = parseInt(item.getValue(), 10);
                if (wantedPageSize != curPageSize) {
                    #{ LogGrid }.store.pageSize = wantedPageSize;
                    #{ LogGrid }.store.reload();
                }
            };
        </script>
    </ext:XScript>
</head>
<body>
    <form runat="server" id="formmain">
        <ext:ResourceManager runat="server">
            <Listeners>
                <DocumentReady Handler="#{TaskManager1}.startTask('reload')" />
            </Listeners>

        </ext:ResourceManager>


        <ext:Hidden ID="GridData" runat="server" />


        <ext:Window
            ID="Window1"
            runat="server"
            Icon="Group"
            Title="Задача на перемещение"
            Width="400"
            Height="600"
            AutoShow="false"
            Modal="true"
            Hidden="true"
            Layout="Fit">
            <Items>


                <ext:FormPanel
                    ID="CompanyInfoTab"
                    runat="server"
                    DefaultAnchor="100%"
                    BodyPadding="10"
                    Padding="5">
                    <Items>
                        <%-- ПЕРЕМЕЩЕНИЕ --%>
                        <ext:TextField ID="VagonNum" runat="server" FieldLabel="Номер вагона" Name="Номер вагона" />
                        

                        <%-- ПО СКЛАДУ --%>
                        <ext:Label ID="OT" runat="server" Html="<h1>ОТ:<h1>" />
                        <ext:ComboBox
                            ID="Prolet"
                            Name="Prolet"
                            runat="server"
                            FieldLabel="Пролет"
                            Editable="false"
                            DisplayField="Text"
                            ValueField="Value"
                            Width="250">
                            <Store>
                                <ext:Store ID="Store1" runat="server">
                                    <Model>
                                        <ext:Model runat="server" IDProperty="Value">
                                            <Fields>
                                                <ext:ModelField Name="Text" />
                                                <ext:ModelField Name="Value" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="#{KranMan}.clearValue();#{KranMan}.getStore().reload();
                                    #{PlaceSklad}.clearValue();
                                    #{PlaceSklad}.getStore().reload();
                                    #{PilaBox}.clearValue();
                                    #{PKBox}.getStore().reload(); #{PKBox}.clearValue();
                                    #{DSAW}.getStore().reload(); #{DSAW}.clearValue();
                                    #{Telega_Forge}.clear();
                                    #{Telega}.clear();
                                    prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="KranMan"
                            Name="KranMan"
                            runat="server"
                            FieldLabel="Кран"
                            Editable="false"
                            DisplayField="KRAN_NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server" AutoLoad="false">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="KRAN_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetKranInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="PlaceSklad"
                            Name="PlaceSklad"
                            runat="server"
                            FieldLabel="Место"
                            Editable="false"
                            DisplayField="NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPlaceInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();#{PlavCombo}.getStore().reload(); #{PlavCombo}.clearValue();" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="PlavCombo"
                            runat="server"
                            FieldLabel="Наименование"
                            Editable="false"
                            DisplayField="PLAV_NAME" 
                            ValueField="COUNT"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="PLAV_NAME"/>
                                                <ext:ModelField Name="COUNT"/>
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPlavInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PLACE" Value="#{PlaceSklad}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();#{PlavCol1}.setValue(App.PlavCombo.getValue());" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="PlavCombo1"
                            runat="server"
                            FieldLabel="Наименование"
                            Editable="false"
                            DisplayField="PLAV_NAME" 
                            ValueField="COUNT"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="PLAV_NAME"/>
                                                <ext:ModelField Name="COUNT"/>
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPlavInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <%--<Parameters>
                                        <ext:StoreParameter Name="PLACE" Value="#{PKBox}.getValue()" Mode="Raw" />
                                    </Parameters>--%>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();#{PlavCol1}.setValue(App.PlavCombo.getValue());" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:NumberField ID="PlavCol1" runat="server" FieldLabel="Количество">
                            
                        </ext:NumberField>
                        <ext:TextField ID="NamePlav" runat="server" FieldLabel="Имя" />
                        <ext:NumberField ID="ColPlav" runat="server" FieldLabel="Количество" />
                        <ext:Label ID="DO" runat="server" Html="<h1>ДО:<h1>" />
                        <ext:ComboBox
                            ID="Prolet1"
                            Name="Prolet1"
                            runat="server"
                            FieldLabel="Пролет"
                            Editable="false"
                            DisplayField="Text"
                            Width="250">
                            <Items>
                                <ext:ListItem Text="Пролет 1" Value="1" />
                                <ext:ListItem Text="Пролет 2" Value="2" />
                                <ext:ListItem Text="Пролет 3" Value="3" />
                            </Items>
                            <Listeners>
                                <Select Handler="#{KranMan1}.clearValue();#{KranMan1}.getStore().reload();#{PlaceSklad1}.clearValue();#{PlaceSklad1}.getStore().reload();#{Telega}.clear();prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <%--<ext:ComboBox
                            ID="Box"
                            Name="Prolet1"
                            runat="server"
                            FieldLabel="Наименование"
                            Editable="false"
                            DisplayField="Text"
                            Width="250">
                            <Items>
                                <ext:ListItem Text="Aus8" Value="1" />
                                <ext:ListItem Text="FlexSteel" Value="2" />
                                <ext:ListItem Text="Ст3пс5" Value="3" />
                            </Items>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>--%>
                        <ext:ComboBox
                            ID="KranMan1"
                            Name="KranMan1"
                            runat="server"
                            FieldLabel="Кран"
                            Editable="false"
                            DisplayField="KRAN_NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="KRAN_NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetKranInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet1}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="PlaceSklad1"
                            Name="PlaceSklad1"
                            runat="server"
                            FieldLabel="Место"
                            Editable="false"
                            DisplayField="NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPlaceInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet1}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>

                        <ext:TextField ID="Telega" runat="server" FieldLabel="Через" Editable="false" />
                        <%-- ПИЛЫ --%>
                        <ext:ComboBox
                            ID="PilaBox"
                            runat="server"
                            FieldLabel="ПК"
                            Editable="false"
                            DisplayField="NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPKInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="#{PKBox}.getStore().reload();prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:ComboBox
                            ID="PKBox"
                            runat="server"
                            FieldLabel="Пила"
                            Editable="false"
                            DisplayField="NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetPK" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{PilaBox}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();#{PlavCombo1}.getStore().reload();" />
                            </Listeners>
                        </ext:ComboBox>
                        <%-- С ПИЛЫ --%>
                        <ext:ComboBox
                            ID="DSAW"
                            runat="server"
                            FieldLabel="Место"
                            Editable="false"
                            DisplayField="NAME"
                            Width="250">
                            <Store>
                                <ext:Store runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="NAME" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Brig.ashx?cmd=AroundSaw" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{Prolet}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>
                        <%-- ПЕЧЬ --%>
                        <ext:ComboBox
                            ID="Telega_Forge"
                            runat="server"
                            FieldLabel="Телега"
                            Editable="false"
                            DisplayField="Text"
                            Width="250">
                            <Items>
                                <ext:ListItem Text="Телега 1" Value="Телега 1 при РМ" />
                                <ext:ListItem Text="Телега 2" Value="Телега 2 при РМ" />
                            </Items>
                            <Listeners>
                                <Select Handler="prov();" />
                            </Listeners>
                        </ext:ComboBox>



                    </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="CancelButton" runat="server" Text="Закрыть" Icon="Cancel">
                    <Listeners>
                        <Click Handler="resetFields(#{CompanyInfoTab});this.up('window').hide()" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="OPP1" runat="server" Text="Подтвердить" StandOut="true">
                    <Listeners>
                        <Click Handler="
                                switch(#{OPP}.getValue()){
                                    case '1':
                                        if(test()){
                                       App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{VagonNum}.getValue(),#{PlaceSklad}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),0,0,{
                                       success: function (result) {App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{VagonNum}.getValue(),#{PlaceSklad}.getValue(),#{KranMan}.getValue(),'1',0);App.direct.Insert_Plav(#{NamePlav}.getValue(),#{PlaceSklad}.getValue(),#{ColPlav}.getValue());
                            SuccessMessageBox('Данные добавлены');
                            resetFields(#{CompanyInfoTab});#{Window1}.hide();
                            }

                            
                                       })
                                       }else{WarningMessageBox('Заполните все поля')}
                                    break;
                                     case '2':
                                        if(test()){
                                            App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{Telega}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),0,0,{

                                     success: function (result) {
                                                                  App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{Telega}.getValue(),#{KranMan}.getValue(),'1',0);
                                                                  App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{Telega}.getValue(),#{PlaceSklad1}.getValue(),#{Prolet1}.getValue(),#{KranMan1}.getValue(),4,result,{
                            
                                                                        success: function (result) {App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{Telega}.getValue(),#{PlaceSklad1}.getValue(),#{KranMan1}.getValue(),'1',0);}
                                                                        });
                                                                 App.direct.Insert_Plav(#{PlavCombo}.getRawValue(),#{PlaceSklad1}.getValue(),#{PlavCol1}.getValue(),{success: function (result){App.direct.update_plav(#{PlaceSklad}.getValue(),#{PlavCombo}.getRawValue(),result,#{PlavCombo}.getValue()); SuccessMessageBox('Данные добавлены');
                                                                 resetFields(#{CompanyInfoTab});#{Window1}.hide();}});
                                                                 
                                                                    
                                                                  },
                                                                 
                                                                
                                     error: function(result){
                            
                                                DangerMessageBox('Данные не добавлены')
                            
                                         }});
                                       }else{WarningMessageBox('Заполните все поля')}
                                    break;
                                    case '3':

                                    App.direct.SAW(#{PKBox}.getValue(),{
                                    success: function (result){
                                    if(result == true){ App.direct.MAXSAW(#{PKBox}.getValue(),{success: function (result){
                                        App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{PKBox}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),0,result,{
                                            success: function(result){
                                            App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{PKBox}.getValue(),#{KranMan}.getValue(),'1',0);
                                            App.direct.Insert_Plav(#{PlavCombo}.getRawValue(),#{PKBox}.getValue(),#{PlavCombo}.getValue())
                                            resetFields(#{CompanyInfoTab});
                                            #{Window1}.hide();
                                            }})}});
                                        }
                                        else {App.direct.MAXSAW(#{PKBox}.getValue(),{success: function (result){
                                            App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{PKBox}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),5,result,{success: function(result){
                                            App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{PlaceSklad}.getValue(),#{PKBox}.getValue(),#{KranMan}.getValue(),'1',0);
                                            App.direct.Insert_Plav(#{PlavCombo}.getRawValue(),#{PKBox}.getValue(),#{PlavCombo}.getValue())
                                            resetFields(#{CompanyInfoTab});
                                            #{Window1}.hide();
                                            }})}});
                                            }
                            
                            
                            }
                            
                            })
                                    
                                    
                                    break;

                                    case '4':
                                        if(test()){
                                       App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{PKBox}.getValue(),#{DSaw}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),0,0,{
                                       success: function (result) {
                                       App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{PKBox}.getValue(),#{DSaw}.getValue(),#{KranMan}.getValue(),'1',0);
                                       App.direct.Insert_Plav(#{PlavCombo1}.getRawValue(),#{DSaw}.getValue(),#{PlavCombo1}.getValue())
                                       SuccessMessageBox('Данные добавлены');resetFields(#{CompanyInfoTab});#{Window1}.hide();}
                            
                                       })
                                       }else{WarningMessageBox('Заполните все поля')}
                                    break;
                                    case '5':
                                        if(test()){
                                       App.direct.Insert(#{OPP}.getSelectedRecord().data.field2,#{DSaw}.getValue(),#{Telega_Forge}.getValue(),#{Prolet}.getValue(),#{KranMan}.getValue(),0,0,{
                                       success: function (result) {
                                       App.direct.LOG(result,#{OPP}.getSelectedRecord().data.field2,#{Dsaw}.getValue(),#{Telega_Forge}.getValue(),#{KranMan}.getValue(),'1',0);
                                       SuccessMessageBox('Данные добавлены');resetFields(#{CompanyInfoTab});#{Window1}.hide();
                                       }
                            
                                       })
                                       }else{WarningMessageBox('Заполните все поля')}
                                    break;
                                    
                            
                                }
                            " />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Close Handler="resetFields(#{CompanyInfoTab});" />
            </Listeners>

        </ext:Window>
        <ext:Viewport runat="server" Layout="Fit">
            <%--<LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" />
            </LayoutConfig>--%>
            <Items>
                <ext:Container Layout="Fit" runat="server">   
                    <Items>
                    <ext:TabPanel
                    runat="server">
                    <Items>
                        <ext:FormPanel runat="server" Title="Задачи " BodyPadding="15" Layout="Fit">
                            <Items>

                                <ext:GridPanel
                                    ID="Grid1"
                                    runat="server"
                                   >
                                    <Store>
                                        <ext:Store ID="StoreBrig1" runat="server">
                                            <Model>
                                                <ext:Model runat="server" IDProperty="id">
                                                    <Fields>
                                                        <ext:ModelField Name="id" />
                                                        <ext:ModelField Name="PartName" />
                                                        <ext:ModelField Name="PartA" />
                                                        <ext:ModelField Name="PartB" />
                                                        <ext:ModelField Name="Kran" />
                                                        <ext:ModelField Name="Kran_Mac" />
                                                        <ext:ModelField Name="Status" />
                                                        <ext:ModelField Name="Status_Name" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>

                                            <Proxy>
                                                <ext:AjaxProxy NoCache="true">
                                                    <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetWheelInfo" />
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                            <Parameters>
                                                <ext:StoreParameter Name="PROLET" Value="#{CheackAll}.checked" Mode="Raw" />
                                            </Parameters>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel runat="server">
                                        <Columns>
                                            <ext:Column runat="server" DataIndex="ID" Hidden="true" />
                                            <ext:Column runat="server" Text="Наименование" DataIndex="NAME" Flex="3">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="NameBox"
                                                        runat="server"
                                                        Icon="Magnifier"
                                                        TriggerAction="All"
                                                        QueryMode="Local"
                                                        DisplayField="Text"
                                                        ValueField="Text">
                                                        <Items>
                                                            <ext:ListItem Text="Перемещение" />
                                                            <ext:ListItem Text="По складу" />
                                                        </Items>
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Text="Со склада" DataIndex="A" Flex="3">
                                                <Items>
                                                    <ext:TextField ID="AFilter" runat="server">
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Text="На место" DataIndex="B" Flex="3">
                                                <Items>
                                                    <ext:TextField ID="BFilter" runat="server">
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Text="Пролет" DataIndex="KRAN" Flex="3">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="ProletBox"
                                                        runat="server"
                                                        DisplayField="Text"
                                                        ValueField="Text">
                                                        <Items>
                                                            <ext:ListItem Text="1" />
                                                            <ext:ListItem Text="2" />
                                                            <ext:ListItem Text="3" />
                                                        </Items>
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Text="Кран" DataIndex="KRAN_MAC" Flex="3">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="KranBox"
                                                        runat="server"
                                                        DisplayField="KRAN_NAME">
                                                        <Store>
                                                            <ext:Store runat="server">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="KRAN_NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:AjaxProxy NoCache="true">
                                                                        <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetKrans" />
                                                                        <ActionMethods Read="POST" />
                                                                        <Reader>
                                                                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                                        </Reader>
                                                                    </ext:AjaxProxy>
                                                                </Proxy>
                                                            </ext:Store>
                                                        </Store>
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" DataIndex="STATUS" Hidden="true" />
                                            <ext:Column runat="server" Text="Статус" DataIndex="STAT" Flex="3">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="StatusBox"
                                                        runat="server"
                                                        DisplayField="NAME">
                                                        <Store>
                                                            <ext:Store runat="server">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:AjaxProxy NoCache="true">
                                                                        <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetStats" />
                                                                        <ActionMethods Read="POST" />
                                                                        <Reader>
                                                                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                                        </Reader>
                                                                    </ext:AjaxProxy>
                                                                </Proxy>
                                                            </ext:Store>
                                                        </Store>
                                                        <Listeners>
                                                            <Change Handler="applyFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <View>
                                        <ext:GridView runat="server" StripeRows="true" />
                                    </View>
                                    <SelectionModel>
                                        <ext:CheckboxSelectionModel runat="server" Mode="single" HeaderWidth="50" />
                                    </SelectionModel>
                                    <BottomBar>
                                        <ext:Toolbar runat="server" ClassicButtonStyle="true" Height="70">
                                            <Items>
                                                <ext:ComboBox
                                                    runat="server"
                                                    ID="OPP"
                                                    FieldLabel="Операция"
                                                    Name="Операция"
                                                    Editable="false"
                                                    DisplayField="Value"
                                                    Width="250">
                                                    <SelectedItems>
                                                        <ext:ListItem Text="Перемещение" Value="1" />
                                                    </SelectedItems>
                                                    <Items>
                                                        <ext:ListItem Text="Перемещение" Value="1" />
                                                        <ext:ListItem Text="ПО складу" Value="2" />
                                                        <ext:ListItem Text="перемещение на ПК" Value="3" />
                                                        <ext:ListItem Text="С ПК на Склад" Value="4" />
                                                        <ext:ListItem Text="РМ-8 в нагревательную печь №1" Value="5" />
                                                    </Items>
                                                </ext:ComboBox>
                                                <ext:Button runat="server" Text="Создать" OnDirectClick="Button2_Click">
                                                </ext:Button>
                                                <ext:Button runat="server" Text="Обновить" OnDirectClick="Reload" />
                                                <ext:Button ID="Button3" runat="server" Text="Подтвердить">
                                                    <Listeners>
                                                        <Click Handler="PilaProv()" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Checkbox runat="server" ID="CheackAll" FieldLabel="Отметить все:">
                                                    <Listeners>
                                                        <Change Handler="#{Grid1}.getStore().reload();" />
                                                    </Listeners>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:Toolbar>
                                    </BottomBar>
                                </ext:GridPanel>
                            </Items>
                        </ext:FormPanel>
                        <ext:FormPanel runat="server" Title="Логи" BodyPadding="15" Layout="Fit">
                            <Items>
                                <ext:Hidden ID="FormatType" runat="server" />
                                <ext:GridPanel
                                    ID="LogGrid"
                                    runat="server"
                                    Height="600">
                                    <Store>
                                        <ext:Store ID="LogStore" runat="server"
                                            RemoteSort="true"
                                            PageSize="10">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="NAME" />
                                                        <ext:ModelField Name="A" />
                                                        <ext:ModelField Name="B" />
                                                        <ext:ModelField Name="KRAN" />
                                                        <ext:ModelField Name="BRIG" />
                                                        <ext:ModelField Name="STATUS" />
                                                        <ext:ModelField Name="DATE_COM" />
                                                        <ext:ModelField Name="ID_ROW" />
                                                        <ext:ModelField Name="DATE_OUT" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>

                                            <Proxy>
                                                <ext:AjaxProxy NoCache="true">
                                                    <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetLog" />
                                                    <ActionMethods Read="GET" />
                                                    <Reader>
                                                        <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                    </Reader>
                                                </ext:AjaxProxy>
                                            </Proxy>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:Column runat="server" Header="Наименование" Text="Наименование" DataIndex="NAME" Flex="3" Sortable="false">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="LogName"
                                                        runat="server"
                                                        Icon="Magnifier"
                                                        TriggerAction="All"
                                                        QueryMode="Local"
                                                        DisplayField="Text"
                                                        ValueField="Text">
                                                        <Items>
                                                            <ext:ListItem Text="Перемещение" />
                                                            <ext:ListItem Text="По складу" />
                                                            <ext:ListItem Text="перемещение на ПК" />
                                                        </Items>
                                                        <Listeners>
                                                            <Change Handler="LogFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Со склада" Text="Со склада" DataIndex="A" Flex="3" Sortable="false">
                                                <Items>
                                                    <ext:TextField ID="LogA" runat="server">
                                                        <Listeners>
                                                            <Change Handler="LogFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Header="На место" Text="На место" DataIndex="B" Flex="3" Sortable="false">
                                                <Items>
                                                    <ext:TextField ID="LogB" runat="server">
                                                        <Listeners>
                                                            <Change Handler="logFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Выполнял" Text="Выполнял" DataIndex="KRAN" Flex="2" Sortable="false">
                                                 <Items>
                                                    <ext:ComboBox
                                                        ID="LogKran"
                                                        runat="server"
                                                        DisplayField="KRAN_NAME">
                                                        <Store>
                                                            <ext:Store runat="server">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="KRAN_NAME" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Proxy>
                                                                    <ext:AjaxProxy NoCache="true">
                                                                        <API Read="~/Handlers/Handler_Brig.ashx?cmd=GetKrans" />
                                                                        <ActionMethods Read="POST" />
                                                                        <Reader>
                                                                            <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                                                        </Reader>
                                                                    </ext:AjaxProxy>
                                                                </Proxy>
                                                            </ext:Store>
                                                        </Store>
                                                        <Listeners>
                                                            <Change Handler="LogFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Проверял" Text="Проверял" DataIndex="BRIG" Flex="2" Sortable="false">
                                                <Items>
                                                    <ext:TextField ID="LogBrig" runat="server">
                                                        <Listeners>
                                                            <Change Handler="LogFilter(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Plugins>
                                                            <ext:ClearButton runat="server" />
                                                        </Plugins>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Column>
                                            <ext:DateColumn runat="server" Header="Создана" Text="Создана" DataIndex="DATE_COM" Format="dd.MM.yyyy H:mm" Flex="3" Sortable="false"/>
                                            <ext:DateColumn runat="server" Header="Выполнена" Text="Выполнена" DataIndex="DATE_OUT" Format="dd.MM.yyyy H:mm" Flex="3" Sortable="false"/>


                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel runat="server" Mode="Multi" />
                                    </SelectionModel>
                                    <View>
                                        <ext:GridView runat="server" StripeRows="true" />
                                    </View>
                                    <TopBar>
                                        <ext:Toolbar runat="server">
                                            <Items>
                                                <ext:DateField
                                                    ID="DateField1"
                                                    runat="server"
                                                    Vtype="daterange"
                                                    FieldLabel="От">
                                                    <CustomConfig>
                                                        <ext:ConfigItem Name="endDateField" Value="DateField2" Mode="Value" />
                                                    </CustomConfig>
                                                    <Listeners>
                                                        <Change Handler="LogFilter(this);" Buffer="250" />
                                                    </Listeners>
                                                    <Plugins>
                                                       <ext:ClearButton runat="server" />
                                                    </Plugins>
                                                </ext:DateField>
                                                <ext:Button ID="Button2" runat="server" Text="В Excel" AutoPostBack="true" OnClick="ToExcel" Icon="PageExcel">
                                                    <Listeners>
                                                        <Click Fn="saveData" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                     <BottomBar>
                                        <ext:Toolbar runat="server" ClassicButtonStyle="true" Height="70">
                                            <Items>
                                            </Items>
                                        </ext:Toolbar>
                                    </BottomBar>
                                </ext:GridPanel>
                            </Items>
                        </ext:FormPanel>
                    </Items>
                </ext:TabPanel>
                    </Items>
                    </ext:Container>
            </Items>
        </ext:Viewport>
        <ext:TaskManager ID="TaskManager1" runat="server">
            <Tasks>
                <ext:Task
                    TaskID="reload"
                    Interval="30000"
                    OnStart="#{Grid1}.getStore().reload();">
                    <Listeners>
                        <Update Handler="#{Grid1}.getStore().reload();#{LogGrid}.getStore().reload();" />
                    </Listeners>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
    </form>
</body>
</html>
