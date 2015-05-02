# Umbraco Personalisation Groups

## What it does

Umbraco Personalisation Groups is an Umbraco package intended to allow personalisation of content to different groups of site visitors.  

It can be downloaded and installed from: https://our.umbraco.org/projects/website-utilities/personalisation-groups

It contains a few different pieces:

- An interface for and various implementations of different personalisation group criteria (e.g. "time of day", "day of week")
- Implementation of the following criteria:
	- Authentication status
    - Cookie key presence/absence and value matching
	- Country (via IP matching)
    - Day of week
    - Session key presence/absence and value matching
    - Time of day
	- Umbraco member group
	- Umbraco member profile field	
	- Umbraco member type
- An extensible mechanism to allow other criteria to be created and loaded from other assemblies
- A property editor with associated angular controllers/views that provide the means of configuring personalisation groups based on the available criteria
- A single extension method on IPublishedContent named ShowToVisitor() that allows checking if the content should be available for the current site visitor

## Status

If you've stumbled across this please be warned it's a very early stage prototype - anything might change at any time.  It's not been used in any production setting and is purely experimental at the moment.

If things work out it'll hopefully be made into an Umbraco package and released on our.umbraco.org, but for now it's only available as source code from this location.

## Set up and use

As this is currently only available as source code there are a few manual steps required to set it up and use it in an Umbraco installation.  Once created as an Umbraco package the necessary document types and data types will be included, but that's not the case at the moment.

Firstly you'll need to download or clone the source code and build it.  Then reference the **Zone.UmbracoPersonalisationGroups.dll** file in an Umbraco project (everything is in this single dll).

### Data types - part 1

Having referenced this dll and started the Umbraco application you should find available a property editor called **Personalisation group definition**.  Create a data type based on this.  There are no pre-values to configure.

### Document types

Firstly I'd suggest setting up a container document type named e.g. *Personalisation Group Folder* - it doesn't need any properties nor a template and should be allowed to be created at the site root or in a "data" folder as appropriate.

Allowable as a child within that should be a second document type named e.g. *Personalisation Group*.  This needs a single instance of the data type that was created above.  The alias for this property must be **definition**.

### Content - part 1

Create an instance of the *Personalisation Group Folder* and one or more *Personalisation Groups* underneath that.  You should find you can configure the personalisation groups for all the available criteria.

Firstly you choose whether to match all or any of the definitions you'll provide.  Then you configure one or more definitions by selecting the appropriate criteria.  The definition is specified as JSON according to a syntax specified by the given criteria.  Currently you'll see some instruction for this, but the easiest way to enter is to use the definition builder that's available via a dialogue box.

![Editing a group definition](/documentation/group-editing.png?raw=true "Editing a group definition")

![Editing a specific criteria](/documentation/definition-editing.png?raw=true "Editing a specific criteria")

### Data types - part 2

Going back to data types, create a new data type based on multi-node tree picker called e.g. *Personalisation group picker*.  Set it's root node to be where you've created the *Personalisation Group Folder* and the types of nodes available to select as just the *Personalisation Groups*.  

### Content - part 2

For any content node you wish to personalise, add a new property of the *Personalisation group picker* with an alias of *personalisationGroups* and select the appropriate groups.

If you don't like this alias you can use a different one, but you will also need to add the following appSetting key to your config file:

    <add key="personalisationGroups.groupPickerAlias" value="myCustomAlias"/> 

### Querying and templating

You'll find that when you work in the templates any reference to an IPublishedContent type has a new extension method called **ShowToVisitor()**.  By calling this you can determine if the current site visitor matches one ore more of the personalisation groups you have associated with the content (which in turn of course depend on the definition for the group you have configured).

So for example, if you have repeated content such as a listing of pages, you can do this to just display the pages relevant for the current site visitor:

		@foreach (var post in Model.Content.Children.Where(x => x.ShowToVisitor()))
		{
			<h2>@post.Name</h2>
		}

With a little more work you can also personalise an individual page.  One way to do this would be to create sub-nodes of a page of a new type called e.g. *Page Variation*.  This document type should contain all the fields common to the parent page that you might want to personalise - e.g. title, body text, image - and an instance of the *Personalisation group picker*.  You could then implement some logic on the parent page template to pull back the first of the sub-nodes that match the current site visitor.  If one is found, you can display the content from that sub-node rather than what's defined for the page.  And if not, display the default content for the page.

## How it works

### Personalisation group criteria (IPersonalisationGroupCriteria)

Group criteria all implement an interface **IPersonalisationGroupCriteria** which provides a few properties to identify and describe the criteria as well as a single method - **MatchesVisitor()**.

Implementations of this interface must provide logic in this method for checking whether the current site visitor matches the definition provided using a JSON syntax supported by the criteria.  So for example the **DayOfWeekPersonalisationGroupCriteria** expects a simple JSON array of day numbers - e.g. [1, 3, 5] - which is compared with the current day to determine a match.

### PersonalisationGroupMatcher

**PersonalisationGroupMatcher** is a static class that when first instantiated will scan all loaded assemblies for implementations of the IPersonalisationGroupCriteria interface and store references to them.  It's in this way the package will support extensions through the development of other criteria that may not be in the core package itself.

It then makes these criteria available to application logic that needs to create group definitions based on them and to check if a given definition matches the related criteria.

### PersonalisationGroupDefinitionPropertyEditor

**PersonalisationGroupDefinitionPropertyEditor** defines an Umbraco property editor for the definition of the personalisation groups.  It has a related angular view and controller, and also ensures the angular assets required for the specific criteria that are provided with the core package are loaded and available for use.

### PersonalisationGroupDefinitionController

**PersonalisationGroupDefinitionController** is a server-side controller that provides logic and resources to the angular view and controller used for the property editor.  It provides JSON end-points for the retrieval of the available criteria via HTTP requests.  It also provides methods for the retrieval of the angular assets that are provided as embedded resources in the package dll (or any extension dlls). See http://www.nibble.be/?p=415 for more detail on how this technique is implemented.

### Angular views and controllers

The primary view and controller for the property editor are **editor.html** and **editor.controller.js** respectively.

In addition to these, each criteria has it's own view and controller that provide a user friendly means of configuring the definitions, named **definition.editor.html** and **definition.editor.controller.js** which are loaded via a call to the Umbraco dialogService.  All are provided as embedded resources.

Each criteria also has an angular service named **definition.translator.js** responsible for translating the JSON syntax into something more human readable.  So again for example the **DayOfWeekPersonalisationGroupCriteria** will render "Sunday, Tuesday, Thursday" from [1, 3, 5].

### PublishedContentExtensions

**PublishedContentExtensions** defines the extension method on **IPublishedContent** named **ShowToVisitor()**.  This implements the following logic:

- Checks for a group picker on the content, if there's not one then we return true (indicating to show to everyone)
- If found get the list of groups picked
- For each group picked, see if the definition provided matches the current site visitor.
    - If any one of them does, we return true (indicating to show the content)
	- If none of them do, we return false (indicating to hide the content)
	
## Notes on particular criteria

### Country

The country criteria uses the [free GeoLite2 IP to country database](http://dev.maxmind.com/geoip/geoip2/geolite2/) made available by Maxmind.com.  It'll look for it in /App_Data/GeoLite2-Country.mmdb or at the path specified in the following appSetting:

    <add key="personalisationGroups.groupPickerAlias" value="myCustomAlias"/> 	

## How to extend it

The idea moving forward is that not every criteria will necessarily be provided by the core package - it should be extensible by developers looking to implement something that might be quite specific to their application.  This should be mostly straightforward.  Due to the fact that the criteria that are made available come from a scan of all loaded assemblies, it should only be necessary to provide a dll with an implementation of **IPersonalisationGroupCriteria** along with the definition editor angular view, controller and translation service - **definition.editor.html**, **definition.editor.controller.js** and **definition.definition.translator.js** respectively - as embedded resources.

There's one gotcha though if you try to use embedded resources as used in the criteria classes in this package. Which is that it's not (I believe) possible to load an angular controller into Umbraco once the back-office application itself has bootstrapped.  The only way to do this is via a property editor, specifically it's **[PropertyEditorAsset]** attribute which is detailed [here](http://issues.umbraco.org/issue/U4-3712).

So in order to have the definition angular controller loaded, it's necessary to create a property editor to do this and have this defined within the extension dll, e.g.:

    /// <summary>
    /// This isn't a "real" property editor, it's rather a hack to inject the angular controller for this criteria definition builder.
    /// The PropertyEditorAsset attribute seems the only way currently to inject additional angular controllers before the application is bootstrapped.
    /// </summary>
    [PropertyEditor("personalisationGroupDefinitionDayOfWeek", "Visitor group definition (day of week - don't use)")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.editor.controller.js")]
	[PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.translator.js")]
    public class DummyDefinitionPropertyEditor : PropertyEditor
    {
    }

It doesn't need to be used (i.e. there's no need to create a data type from it).  And so the only downside if that you have this property editor available in the drop-down list for creating data types that you don't need.  

## Planned next steps

The following tasks are planned to continue development of this package:

- Release as a package on our.umbraco.org
- Update member group and type definition editors to use drop-down list rather than free text entry

## Troubleshooting/known issues

If you run into a problem with the data type failing to load when running with debug="false", this is because it's necessary to whitelist the domains in use.  See the [forum post here](https://our.umbraco.org/forum/umbraco-7/developing-umbraco-7-packages/64459-Single-file-property-editor-and-debug=false) along with links for discussion and resolution details.  In summary though:

- Open Config\ClientDependency.config
- Find the **bundleDomains** attribute
- Add a comma separated list of the domains you are using