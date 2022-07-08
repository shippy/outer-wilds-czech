# Outer Wilds Czech Localization

Replaces the English text in the game with Czech. No configuration necessary.

Based on [Outer Wilds Korean Translation](https://outerwildsmods.com/mods/outerwildskoreantranslation/) and [Outer Wilds Traditional Chinese Translation](https://outerwildsmods.com/mods/outerwildstraditionalchinesetranslation/).

## Spotted a mistake in the localization?

1. Check the [issues](https://github.com/shippy/outer-wilds-czech/issues) to see if it's already being worked on;
2. If not, file an issue.
3. Consider [contributing a fix](CONTRIBUTING).

## Installation

### Released version

**When we have one,** it should be as easy as:

1. Download and install the [Outer Wilds Mod Manager](https://outerwildsmods.com/mod-manager/)
2. Select "Czech Localization" from the list of mods
3. Launch using the Mod Manager rather than your launcher of choice

### Development version

1. Download and install the [Outer Wilds Mod Manager](https://outerwildsmods.com/mod-manager/),
2. Download and install the [Visual Studio Community Version](https://visualstudio.microsoft.com/),
3. Install OWML in the Mod Manager (but **not** the released Czech Localization),
4. Clone this repository,
5. Open the folder in Visual Studio,
6. Check that `CzechTranslation.csproj.user` is pointing at the right location for your OWMM installation,
7. Build the project (per `CzechTranslation.sln`),
8. Launch the Mod Manager, ensure "Czech Localization" is selected, and launch the game.

## Contributors / Thanks

- Václav Soukup for brainstorming the names of planetary bodies with me + translating the UI
- Agnieszka Buchtová for Ilex's rhymes
- @xen-42 for creating the mod to sub in the Czech text in English's stead
- @MegaPiggy for selflessly contributing a fix we didn't realize we'd needed