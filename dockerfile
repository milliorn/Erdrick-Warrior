# build the module file
FROM index.docker.io/nwntools/nasher:0.15.0 AS moduleBuild
ADD . /src/moduleBuild/
WORKDIR /src/moduleBuild
RUN nasher pack

# Pull Dotnet image to build the project
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ADD ./src/services /Build
# build EldritchWarrior code
WORKDIR /Build/EldritchWarrior/
RUN dotnet publish -c Release -o out 

# Build the final NWN server image
FROM nwndotnet/anvil:8193.26.0-rc.6
# copy module
COPY --from=moduleBuild /src/moduleBuild/*.mod /nwn/data/data/mod
# copy built dll's
COPY --from=build /Build/EldritchWarrior/out /nwn/anvil/Plugins/EldritchWarrior/

ENV NWN_SERVERNAME=DotnetTest \
  NWN_MODULE=Eldritch_Warrior \
  NWN_PUBLICSERVER=0 \
  NWN_AUTOSAVEINTERVAL=0 \
  NWN_DIFFICULTY=3 \
  NWN_ELC=1 \
  NWN_GAMETYPE=0 \
  NWN_ILR=1 \
  NWN_MAXCLIENTS=255 \
  NWN_MINLEVEL=1 \
  NWN_MAXLEVEL=40 \
  NWN_ONEPARTY=0 \
  NWN_PAUSEANDPLAY=0 \
  NWN_PORT=5121 \
  NWN_PVP=2 \
  NWN_RELOADWHENEMPTY=0 \
  NWN_SERVERVAULT=1 \
  NWN_DMPASSWORD=test\
  # managed required
  # NWNX_DOTNET_ASSEMBLY=/nwn/Dotnet/NWN.Managed \
  NWM_NLOG_CONFIG=/nwn/home/nlog.config \
  # nwnx env
  NWNX_ADMINISTRATION_SKIP=n \
  NWNX_APPEARANCE_SKIP=n \
  NWNX_AREA_SKIP=n \
  NWNX_CHAT_SKIP=n \
  NWNX_CREATURE_SKIP=n \
  NWNX_DAMAGE_SKIP=n \
  NWNX_DIALOG_SKIP=n \
  NWNX_DOTNET_SKIP=n \
  NWNX_ELC_SKIP=n \
  NWNX_ENCOUNTER_SKIP=n \
  NWNX_EVENTS_SKIP=n \
  NWNX_FEEDBACK_SKIP=n \
  NWNX_ITEM_SKIP=n \
  NWNX_ITEMPROPERTY_SKIP=n \
  NWNX_OBJECT_SKIP=n \
  NWNX_PLAYER_SKIP=n \
  NWNX_QUICKBARSLOT_SKIP=n \
  NWNX_RACE_SKIP=n \
  NWNX_REDIS_SKIP=y \
  NWNX_RENAME_SKIP=n \
  NWNX_REVEAL_SKIP=n \
  NWNX_SKILLRANKS_SKIP=n \
  NWNX_UTIL_SKIP=n \
  NWNX_VISIBILITY_SKIP=n \
  NWNX_WEAPON_SKIP=n \
  NWNX_COMBATMODES_SKIP=n \
  # NWNX TWEAKS
  NWNX_TWEAKS_SKIP=n \
  NWNX_TWEAKS_DISABLE_PAUSE=true \
  NWNX_TWEAKS_DISABLE_QUICKSAVE=true \
  NWNX_TWEAKS_PARRY_ALL_ATTACKS=true \
  NWNX_TWEAKS_FIX_GREATER_SANCTUARY_BUG=true \
  NWNX_TWEAKS_HIDE_CLASSES_ON_CHAR_LIST=true \
  NWNX_TWEAKS_PRESERVE_ACTIONS_ON_DM_POSSESS=true \
  NWNX_TWEAKS_DISABLE_MONK_ABILITIES_WHEN_POLYMORPHED=true \
  NWNX_TWEAKS_FIX_UNLIMITED_POTIONS_BUG=true \
  NWNX_TWEAKS_BLOCK_DM_SPAWNITEM=true \
  NWNX_TWEAKS_PLAYER_DYING_HP_LIMIT=-127
