import { ApplicationConfig, provideBrowserGlobalErrorListeners, LOCALE_ID, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import Aura from '@primeuix/themes/aura';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptor/auth-interceptor';

import { IconsLucide } from './shared/utils/lucide-icons/lucide-icons';
import { LucideAngularModule } from 'lucide-angular';


registerLocaleData(localePt);


export const appConfig: ApplicationConfig = {
  providers: [
        provideBrowserGlobalErrorListeners(),
        provideRouter(routes),
        provideAnimationsAsync(),
        { provide: LOCALE_ID, useValue: 'pt-BR' },

        providePrimeNG({
            theme: {
                preset: Aura
            },
        }),
        provideHttpClient(
          withInterceptors([AuthInterceptor])
        ),
        importProvidersFrom(
          LucideAngularModule.pick(IconsLucide)
        )

  ]
};
