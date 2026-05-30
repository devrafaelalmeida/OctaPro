import { themeQuartz, iconSetQuartzLight } from 'ag-grid-community';

// to use myTheme in an application, pass it to the theme grid option
export const AG_GRID_THEME_CUSTOM = themeQuartz
    .withPart(iconSetQuartzLight)
    .withParams({
        accentColor: "#102D81",
        backgroundColor: "#F5F8FF",
        borderRadius: 0,
        browserColorScheme: "light",
        cellTextColor: "#03082A",
        columnBorder: false,
        fontFamily: {
            googleFont: "Inter"
        },
        fontSize: 14,
        foregroundColor: "#000000",
        headerBackgroundColor: "#0C368F",
        headerFontFamily: {
            googleFont: "Inter"
        },
        headerFontSize: 14,
        headerFontWeight: 700,
        headerRowBorder: true,
        headerTextColor: "#FFFFFF",
        oddRowBackgroundColor: "#EEF3FF",
        rowBorder: true,
        wrapperBorder: true,
        wrapperBorderRadius: 0
    });


