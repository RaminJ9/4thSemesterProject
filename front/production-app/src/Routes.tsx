export const MyRoutes = {
  production: "/" as const,
  info: "/info" as const,
  machineInfo: "/info/:guid" as const,
  machineInfoPath: (guid: string | undefined ) => `/info/${guid}`,
};