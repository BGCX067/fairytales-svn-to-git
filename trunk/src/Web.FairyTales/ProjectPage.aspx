<%@ Page Language="C#" MasterPageFile="~/FairyTales.Master" AutoEventWireup="true"
    CodeBehind="ProjectPage.aspx.cs" Inherits="Web.FairyTales.ProjectPage" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    function toggleCreateProjectControls()
    {
        var control = document.getElementById('<%=createProjectControls.ClientID %>');
        control.style.display = control.style.display == 'inline' ? 'none' : 'inline';       
    }    
</script>

    <asp:UpdatePanel ID="storyUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="ContentPanel" runat="server" CssClass="content">
                <asp:Panel ID="ProjectPanel" runat="server" CssClass="projectPanel">                    
                    <asp:Image ID="Plus" runat="server" ImageUrl="~/App_Themes/Default/Images/plus.gif" AlternateText="Show/Hide Add Project" onclick="javascript:toggleCreateProjectControls()" />
                                        
                    <asp:Label ID="createProjectControls" runat="server" style="display:none">
                        <asp:TextBox ID="projectInput" runat="server" />
                        <asp:Button ID="addProjectButton" runat="server" Text="Add Project" OnClick="addProjectButton_Click" />
                        <asp:Label ID="errorMessageLabel" runat="server" ForeColor="Blue" />
                    </asp:Label>
                    <asp:DropDownList ID="projectChooser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="projectChooserIndexChanged" />
                </asp:Panel>
                
                <asp:Label ID="activeProjectLabel" runat="server" CssClass="activeProject" Text="" />
                                
                <asp:Panel ID="ProjectViewPanel" runat="server">
                    <ajaxToolkit:TabContainer runat="server" ID="contentSwitcher">
                        <ajaxToolkit:TabPanel runat="server" HeaderText="StoryBoard" ID="StoriesTab">
                            <HeaderTemplate>
                                StoryBoard
                            </HeaderTemplate>
                            <ContentTemplate>
                            <asp:Panel ID="StoryPanel" runat="server" CssClass="storyPanel">
                                <asp:Panel ID="StoryTablePanel" runat="server" CssClass="storyTable">
                                    <asp:GridView ID="storyTable" runat="server" GridLines="None" AllowPaging="True"
                                        OnRowDataBound="BindRow" AutoGenerateEditButton="True" OnRowDeleting="storyTable_DeleteRow"
                                        OnRowEditing="storyTable_EditRow" OnRowUpdating="storyTable_UpdateRow" OnRowCancelingEdit="storyTable_CancelingEdit"
                                        OnPageIndexChanging="storyTable_PageIndexChanging" 
                                        AutoGenerateColumns="False">                                
                                        <PagerStyle CssClass="tablePager" />
                                        <Columns>
                                            <asp:BoundField DataField="Abbreviation" ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Story Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="storyName_label" runat="server" Text='<%# Eval("Name") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="storyName_edit" runat="server" Text='<%# Eval ("Name") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Priority">
                                                <ItemTemplate>
                                                    <asp:Label ID="importance_label" runat="server" Text='<%# Eval("Importance") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="importance_edit" Columns="5" runat="server" Text='<%# Eval ("Importance") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estimate">
                                                <ItemTemplate>
                                                    <asp:Label ID="estimate_label" runat="server" Text='<%# Eval("Estimate") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="estimate_edit" Columns="5" runat="server" Text='<%# Eval ("Estimate") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="status_label" runat="server" Text='<%# Eval("ActiveStatus") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="status_dropdown" AutoPostBack="true" DataSource='<%#StoryStatuses %>'
                                                        SelectedValue='<%# Eval("ActiveStatus") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="How To Demo">
                                                <ItemTemplate>
                                                    <asp:Label ID="howToDemo_label" runat="server" Text='<%# Eval("HowToDemo") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="howToDemo_edit" TextMode="MultiLine" runat="server" Text='<%# Eval ("HowToDemo") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="delete_button" CommandName="storyTable_DeleteRow" OnClick="storyTable_DeleteRow" 
                                                    CommandArgument='<%# Eval("Abbreviation") %>'>
                                                    <asp:Image ID="wings" runat="server" ImageUrl="~/App_Themes/Default/Images/delete.gif" AlternateText="delete" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    Hide:
                                    <asp:CheckBoxList runat="server" ID="statusFilter" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="storyTable_FilterStories" AutoPostBack="True">
                                        <asp:ListItem Text="Not Started" />
                                        <asp:ListItem Text="Started" />
                                        <asp:ListItem Text="Paused" />
                                        <asp:ListItem Text="Done" />
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:TextBox ID="storyInput" runat="server" ></asp:TextBox>
                            <asp:Button ID="addStoryButton" runat="server" Text="Add Story" OnClick="addStoryButton_Click" />        
                            </ContentTemplate>    
                        </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>            
                    
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
