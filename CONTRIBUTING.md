# Contributing guidelines

Thank you for deciding to help!

## Technical contributions

You probably know what to do - standard Github workflow. We'll revisit this section if we ever have a problem with that.

### Building the mod

See [README](README.md) instructions on installing a development version of the locatization.

### Making a release

1. Increase the point version in `manifest.json` in rough accordance with [semantic versioning](https://semver.org). In practice, for X.Y.Z, change Z for minor typos and proofreads, Y for bigger adjustments, and X for major points in the translation. (We aim to be done at 1.0.0, but might need 1.0.x.)
2. Commit the change to `manifest.json` and push it to Github's `master` branch.
3. **On your local computer:** 
	1. Build the mod,
	2. In the Mod Manager, find your mod, click the three dots menu button, and select "Show in explorer". You should see the directory to where your mod was built. 
	3. Create a **zip** that includes all these files. This will be your release. Has to be **zip** specifically.
4. On GitHub:
	1. Go to [the Releases page](https://github.com/shippy/outer-wilds-czech/releases) of `outer-wilds-czech`. Press "Draft a new release".
	2. In the "Tag version" field, insert the same mod version that you included in `manifest.json` in Step 1. It's very important that the release tag version is the same as the `version` field in the `manifest.json` inside the zip, otherwise your mod will always show as outdated in the Mod Manager.
	3. Release title and description are up to you.
	4. Add your zip to the release as a binary by drag & dropping the file to the release assets area (or just click "Attach binaries by etc etc" and select your file). Make sure you only upload one zip file, since anything after the first one will be ignored by the mod database / mod manager.
	5. Press "Publish release".


## Translation contributions

### Principy překladu

Učinili jsme následující volby - pokud s nimi nesouhlasíte, prosím otevřete Issue nebo diskusní vlákno v Discussions. _Kurzívou_ jsou uvedené volby, které aktivně přehodnocujeme.

1. Používáme následující překlady planet:

	| English          | Czech              |
	|------------------|--------------------|
	| sun              | slunce (s malým s) |
	| Sun Station      | Sluneční stanice   |
	| Hourglass Twins  | Přesýpací dvojčata |
	| Ember Twin       | Doutnák            |
	| Ash Twin         | Popelák            |
	| Timber Hearth    | Domopec            |
	| Brittle Hollow   | Křehkodutá planeta |
	| Hollow's Lantern | Křehkodutý měsíc   |
	| Giant's Deep     | Obří hlubina       |
	| Dark Bramble     | Temnotrn           |
	| Interloper       | Tulák              |
	| Stranger         | Cizinec            |
	| Quantum Moon     | Kvantový měsíc     |

2. Používáme následující překlady míst a věcí:

	| English              | Czech                     |
	|----------------------|---------------------------|
	| Sunless City         | Neslunné město            |
	| Hanging City         | Visací město              |
	| Ash Twin Project     | Popelákový projekt        |
	| Orbital Probe Cannon | _Orbitální sondový kanón_ |
	| Vessel               | Plavidlo                  |
	| Escape Pod           | Záchranný člun            |
	| Anglerfish           | Ďas                       |
	| Black Hole Forge     | Černoděrá kovárna         |
	| White Hole Station   | Běloděrá stanice          |
	| Ghost Matter         | Antihmota                 |

3. _Nepřekládáme jména postav._
4. Slovo "Nomai" zůstává nepřeloženo, je nesklonné a vyskytuje se pouze jako podstatné jméno (v případě nutnosti přídavného jména používáme genitiv, např. "Nomai Shuttle" = "loď Nomai").
5. Domopecané říkají "sluneční soustava", Nomai říkají "planetární systém".
6. _Pohlaví je Domopecanům přiřazeno podle jména._

### Jak začít překládat

Postupujte podle pokynů v README, abyste rozchodili místní instalaci - ale předtím, než si naklonujete repozitář, si jej prosím forkněte (tlačítko Fork v pravém horním rohu).

Všechny překlady jsou v `CzechTranslation/translations/czech.xml`. Lze jej editovat v každém textovém editoru. V něm:

1. Nikdy neměňte obsah v tagu `<key />` (protože to je klíč, podle kterého Outer Wilds vyhledává lokalizaci) - ani whitespace, tj. mezery nebo tabulátory!
2. Měňte jen obsah v tagu `<value />`.

Abyste svůj překlad dostali zpět do repozitáře:

1. Přidejte (`git add`) a commitněte (`git commit`) své změny do svého forknutého repozitáře,
2: Otevřete Pull Request pro tento repozitář.