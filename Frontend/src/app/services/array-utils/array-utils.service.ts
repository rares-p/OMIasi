import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class ArrayUtilsService {
    arrayBufferToBase64(buffer: Uint8Array): string {
        const binaryString = Array.from(buffer)
            .map((byte) => byte.toString(2).padStart(8, '0'))
            .join(' ');

        return binaryString;
    }

    base64ToArrayBuffer(base64: string): Uint8Array {
        const binaryString = window.atob(base64);
        const len = binaryString.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        return bytes;
    }
}
