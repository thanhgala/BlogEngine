import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { RootModule } from './app/root.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName("base")[1].href;
}

const providers = [
  { provide: "BASE_URL", useFactory: getBaseUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(RootModule)
  .catch(err => console.error(err));

