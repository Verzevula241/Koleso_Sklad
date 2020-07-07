<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kran.aspx.cs" Inherits="Koleso.Kran" %>

<%@ Import Namespace="System.Collections.Generic" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Задачи</title>

    <link href="/resources/css/examples.css" rel="stylesheet" />
    <script>

       

    
        function sprite(form) {
            
            var selection = form.getSelection();
            let i = 0;
            if (selection.length != 0) {
                App.direct.HideALL();
                selection.forEach((element) => {
                    App.direct.lable(element.data.A, element.data.B, i, {
                        eventMask: {
                            minDelay: 100
                        }
                    })
                    i++
                });
            } else App.direct.HideALL();
           
        };
        var bindWidgets = function (column, cmp, record) {
            var id = record.get('ID');
            var name = record.get('PartName');
            var kran = App.hKran.getValue();
            debugger;
            Ext.Msg.show({
                title: 'Добавление данных',
                msg: 'Вы пытаетесь подтвердить выполнение. Вы уверены?',
                buttons: Ext.Msg.YESNO,
                fn: function (btn, text) {
                    if (btn == 'yes') {
                        App.direct.update(id, 2, kran, {
                            success: function (result) {

                                reloadKeepingPage(App.GridPanel2);
                               
                            }});
                    }
                },
                icon: Ext.MessageBox.QUESTION
            });

        };
        var CreateSprite = function (column, cmp, record) {
            debugger;
            App.direct.task(cmp[0].data.PartB);

        };
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
        function reloadKeepingPage(grid) {
            debugger;
            var store = grid.getStore();
            store.reload();
        }
    </script>
</head>
<body>
    <form runat="server" id="formmain">
        <ext:ResourceManager runat="server" />
         <ext:Viewport runat="server">
             <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" />
            </LayoutConfig>
             <Items>
        <ext:label runat="server" ID="MAC"></ext:label>
        <ext:Hidden runat="server" ID="hProlet" />
        <ext:Hidden runat="server" ID="hKran" />
        <ext:TabPanel
            runat="server">
            <Defaults>
                <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                <ext:Parameter Name="scrollable" Value="both" />
            </Defaults>
            <Items>
                <ext:FormPanel runat="server" Title="Лист Задач">
                    <Items>
                        <ext:GridPanel
                            ID="GridPanel1"
                            runat="server">
                            <TopBar>
                                <ext:Toolbar runat="server" Layout="HBoxLayout" Margin="1">
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="Store1" runat="server" PageSize="10" AutoLoad="false">
                                    <Model>
                                        <ext:Model runat="server" IDProperty="ID_WORK">
                                            <Fields>
                                                <ext:ModelField Name="ID_WORK" />
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="NAME" />
                                                <ext:ModelField Name="A" />
                                                <ext:ModelField Name="B" />
                                                <ext:ModelField Name="STATUS" />

                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Kran.ashx?cmd=GetWorkInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{hProlet}.getValue()" Mode="Raw" />
                                        <ext:StoreParameter Name="KRAN_MAC" Value="#{hKran}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="ID" Hidden="true" />
                                    <ext:Column runat="server" Text="Наименование" DataIndex="NAME" Flex="3" />
                                    <ext:Column runat="server" Text="Со склада" DataIndex="A" Flex="3"/>
                                    <ext:Column runat="server" Text="На место" DataIndex="B" Flex="3"/>
                                    <ext:Column runat="server" DataIndex="STATUS" Hidden="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" Mode="Multi" HeaderWidth="50" />
                            </SelectionModel>

                            <BottomBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:Button ID="Button1" runat="server" Text="Взять в работу" StandOut="true">
                                            
                                            <Listeners>
                                                <Click Handler="App.direct.SubmitSelection(Ext.encode(#{GridPanel1}.getRowsValues({selectedOnly:true})),{
                                                    success: function (result) {reloadKeepingPage(#{GridPanel1});reloadKeepingPage(#{GridPanel2});}
                                                    })" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="Button2" runat="server" Text="Обновить" StandOut="true">
                                            
                                            <Listeners>
                                                <Click Handler="" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>

                        </ext:GridPanel>

                        <ext:Label ID="Label1" runat="server" />
                    </Items>
                </ext:FormPanel>
                <ext:FormPanel runat="server" Title="Карта склада" ID="fpMap">
                    <Items>
                        <ext:DrawContainer ID="Draw1" runat="server" Height="400" UIName="DrawCon" />
                        <ext:Label runat="server" ID="lb1" />
                        <ext:GridPanel
                            ID="GridPanel2"
                            runat="server"
                            Flex="1"
                            ForceFit="true">
                            <Store>
                                <ext:Store ID="Store2" runat="server" PageSize="5">
                                    <Model>
                                        <ext:Model runat="server" IDProperty="ID">
                                            <Fields>
                                                <ext:ModelField Name="ID" />
                                                <ext:ModelField Name="id" />
                                                <ext:ModelField Name="A" />
                                                <ext:ModelField Name="B" />
                                                <ext:ModelField Name="STATUS" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:AjaxProxy NoCache="true">
                                            <API Read="~/Handlers/Handler_Kran.ashx?cmd=GetJobInfo" />
                                            <ActionMethods Read="POST" />
                                            <Reader>
                                                <ext:JsonReader RootProperty="data" TotalProperty="total" />
                                            </Reader>
                                        </ext:AjaxProxy>
                                    </Proxy>
                                    <Parameters>
                                        <ext:StoreParameter Name="PROLET" Value="#{hProlet}.getValue()" Mode="Raw" />
                                        <ext:StoreParameter Name="KRAN_MAC" Value="#{hKran}.getValue()" Mode="Raw" />
                                    </Parameters>
                                </ext:Store>

                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="id" Hidden="true" />
                                    <ext:Column runat="server" Text="Со склада" DataIndex="A" Flex="3"/>
                                    <ext:Column runat="server" Text="На место" DataIndex="B" Flex="3" />
                                    <ext:Column runat="server" Text="На место" DataIndex="STATUS" Hidden="true"/>
                                    <ext:CommandColumn ColumnID="Commands" Header="Операции" runat="server" Flex="1">
                                        <Commands>
                                            <ext:GridCommand CommandName="AddRecord" ToolTip-Text="Подтвердить" Icon="Accept" Text="Подтвердить" MinWidth="50"/>
                                        </Commands>
                                        <Listeners>
                                            <Command Handler="function(item,command,record){bindWidgets(item,command, record);}" />
                                        </Listeners>
                                    </ext:CommandColumn>

                                </Columns>
                            </ColumnModel>
                            <View>
                                <ext:GridView runat="server" StripeRows="true" />
                            </View>
                            
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" Mode="Multi" HeaderWidth="50"/>
                            </SelectionModel>
                            <Listeners>
                               <%-- <SelectionChange Handler="function(item,command,record){CreateSprite(item,command,record)}" Buffer="10" />--%>
                                <SelectionChange Handler="sprite(#{GridPanel2})" Buffer="10" />
                            </Listeners>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:TabPanel>
                 </Items>
             </ext:Viewport>
    </form>
    <ext:TaskManager ID="TaskManager2" runat="server">
            <Tasks>
                <ext:Task
                    TaskID="reload"
                    Interval="30000"
                    OnStart="#{GridPanel1}.getStore().load();">
                    <Listeners>
                        <Update Handler="reloadKeepingPage(#{GridPanel1});reloadKeepingPage(#{GridPanel2});"/>
                    </Listeners>
                </ext:Task>
            </Tasks>
        </ext:TaskManager>
</body>
</html>
